using ApiAuth;
using ApiAuth.Application.Services;
using ApiAuth.Domain.Models;
using ApiAuth.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var key = Encoding.ASCII.GetBytes(Settings.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("admin", policy => policy.RequireRole("manager"));
        options.AddPolicy("Employee", policy => policy.RequireRole("employee"));
    }); 

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", (User model) =>
{
    var user = UserRepository.Auth(model.Username, model.Password);

    if (user == null)
    {
        return Results.NotFound(new {
            message = "Usuário ou senha inválido"
        });
    }

    var token = TokenService.GenerateToken(user);

    user.Password = "";

    return Results.Ok(new 
    {
        user = user,
        token = token
    });

})
.WithName("login");

app.MapGet("/anonymous", () => 
{
    Results.Ok("anonymous");
}).AllowAnonymous();

app.MapGet("/authenticated", (ClaimsPrincipal user) =>
{
    Results.Ok(new {message = $"authenticated as {user.Identity.Name}"});
}).RequireAuthorization();

app.MapGet("/employee", (ClaimsPrincipal user) =>
{
    Results.Ok(new { message = $"authenticated as {user.Identity.Name}" });
}).RequireAuthorization("Employee");

app.MapGet("/manager", (ClaimsPrincipal user) =>
{
    Results.Ok(new { message = $"authenticated as {user.Identity.Name}" });
}).RequireAuthorization("Admin");
app.Run();
