using FirstBlazorApp.Data;
using ForumAdminPanel.Models;
using Microsoft.EntityFrameworkCore;
using FirstBlazorApp.Auth;
namespace FirstBlazorApp.Services
{
    public class UserAccountService
    {
        private IDbContextFactory<appDbContext> _dbContextFactory;


        public UserAccountService(IDbContextFactory<appDbContext> dbContextFactory) 
        {
            _dbContextFactory = dbContextFactory;
        }


        public User GetCurrentUser()
        {


            return null;
        }

        public User? GetByUserName(string userName) 
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var foundedUser=context.Users.FirstOrDefault(U=>U.UserName==userName);
                return foundedUser;
            }
        }

        public void RegisterUser(User user) 
        {
            using ( var context = _dbContextFactory.CreateDbContext())
            {
                bool doesUserExist = context.Users.Any(U => U.UserName == user.UserName && U.Email == user.Email);

                if (doesUserExist)
                {
                    
                }
                else
                {
                    User registeredUser = new User
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Password = user.Password,
                        RegisteredDate = DateTime.UtcNow,
                        UserStatus = 3
                    };

                    context.Users.Add(registeredUser);
                    context.SaveChanges();
                }
            }
        }
    }
}
