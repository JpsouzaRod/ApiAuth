using ApiAuth.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAuth.Adapter.Data
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions option): base(option) { }

        public DbSet<User> User{ get; set; }
    }
}
