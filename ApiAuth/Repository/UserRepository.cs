using ApiAuth.Domain.Models;

namespace ApiAuth.Repository
{
        
    public static class UserRepository
    {
        public static User Auth(string username, string password)
        {
            var users = new List<User>
            {
                new User() {Id = 1, Username = "batman", Password = "teste@123", Role = "manager"},
                new User() {Id = 2, Username = "robin", Password = "teste", Role = "employee"}
            };

            return users
                    .FirstOrDefault(x => 
                        string.Equals(x.Username,username) && 
                        x.Password == password);
        }
    }
}   
