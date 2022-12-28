using ApiAuth.Adapter.Data;
using ApiAuth.Application.Services;
using ApiAuth.Domain.Models;
using System.Security.Cryptography;
using System.Security.Policy;

namespace ApiAuth.Repository
{
    public interface IUserRepository
    {
        Task RegistrarUsuario(User user);
        Task RedefinirSenha(User user, string password);
        Task<User> AutenticarUsuario(string username, string password);
    }
        
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext context;
        private readonly HashService hash;
        public UserRepository(UserDbContext _context)
        {
            context = _context;
            hash = new HashService(SHA512.Create());
        }

        public async Task<User> AutenticarUsuario(string username, string password)
        {
            var user = context.User.FirstOrDefault(x => 
                x.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                x.Password.Equals(hash.CriptografarSenha(password), StringComparison.OrdinalIgnoreCase));

            return user;
        }

        public async Task RedefinirSenha(User user, string password)
        {
            var userReplace = context.User.FirstOrDefault(x =>
                x.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase));

            userReplace.Password = hash.CriptografarSenha(password);

            context.User.Update(userReplace);
            context.SaveChanges();
        }

        public async Task RegistrarUsuario(User user)
        {
            user.Password = hash.CriptografarSenha(user.Password);  
            context.User.AddAsync(user);
            context.SaveChanges();

        }
    }
}   
