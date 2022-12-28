using ApiAuth.Application.Services;
using ApiAuth.Domain.Models;
using ApiAuth.Repository;
using System.Security.Claims;

namespace ApiAuth.Endpoint
{
    public static class Endpoints
    {
        public static void AddEndpoints(this WebApplication app)
        {
            app.UseEndpoints(options =>
            {
                app.MapPost("/login", async (User model, IUserRepository repository) =>
                {
                    var user = await repository.AutenticarUsuario(model.Username, model.Password);

                    if (user == null)
                    {
                        return Results.NotFound(new
                        {
                            message = "Usuário ou senha inválido"
                        });
                    }

                    var token = TokenService.GenerateToken(user);

                    return Results.Ok(new
                    {
                        user = user,
                        token = token
                    });

                }).WithName("login");

                app.MapPost("/Register", async (User model, IUserRepository repository) =>
                {
                    await repository.RegistrarUsuario(model);
                    Results.Ok("Cadastro feito com sucesso");
                }).AllowAnonymous();

                app.MapGet("/authenticated", (ClaimsPrincipal user) =>
                {
                    Results.Ok(new { message = $"authenticated as {user.Identity.Name}" });
                }).RequireAuthorization();

                app.MapGet("/employee", (ClaimsPrincipal user) =>
                {
                    Results.Ok(new { message = $"authenticated as {user.Identity.Name}" });
                }).RequireAuthorization("Employee");

                app.MapGet("/manager", (ClaimsPrincipal user) =>
                {
                    Results.Ok(new { message = $"authenticated as {user.Identity.Name}" });
                }).RequireAuthorization("Admin");


            });
        }
    }
}
