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

        public User? GetByUserName(string userName) 
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var foundedUser=context.Users.FirstOrDefault(U=>U.UserName==userName);
                return foundedUser;
            }
        }

        public User RegisterUser(User user) 
        {   
            if(user != null)
            {
                using ( var context = _dbContextFactory.CreateDbContext())
                {
                    bool doesUserExist = context.Users.Where(U => U.UserName == user.UserName && U.Email == user.Email).Any();

                    if (doesUserExist)
                    {
                        return null;
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

                        return registeredUser;
                    }
                }
            }
            else
            {
                return null;
            }
            
        }
    }
}
