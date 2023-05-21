using FirstBlazorApp.Data;
using ForumAdminPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstBlazorApp.Services
{
    public class UserAccountService
    {
        private IDbContextFactory<appDbContext> _dbContextFactory;


        public UserAccountService(IDbContextFactory<appDbContext> dbContextFactory) 
        {
            _dbContextFactory = dbContextFactory;
        }

        private User? GetByUserName(string userName) 
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var foundedUser=context.Users.FirstOrDefault(U=>U.UserName==userName);
                return foundedUser;
            }


        }
    }
}
