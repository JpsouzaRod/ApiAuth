using ApiAuth.Domain.Models;
using ApiAuth.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiAuth.Adapter.Mongo
{
    public class MongoRepository : IUserRepository
    {
        private readonly IMongoCollection<User> usersCollection;
        public MongoRepository(IOptions<MongoOptions> _options) 
        {
            var mongoClient = new MongoClient(_options.Value.ConnectionString);
            usersCollection = mongoClient.GetDatabase(_options.Value.Database)
                .GetCollection<User>(_options.Value.CollectionName);
        }

        public Task<User> AutenticarUsuario(string username, string password)
        {

            throw new NotImplementedException();
        }

        public Task RedefinirSenha(User user, string password)
        {
            throw new NotImplementedException();
        }

        public Task RegistrarUsuario(User user)
        {
            throw new NotImplementedException();
        }
    }
}
