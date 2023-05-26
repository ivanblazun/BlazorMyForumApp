using FirstBlazorApp.Data;
using FirstBlazorApp.Pages.Users;
using ForumAdminPanel.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace FirstBlazorApp.Services
{
    public class UserService
    {
        private IDbContextFactory<appDbContext> _dbContextFactory;
        private readonly ProtectedSessionStorage _sessionStorage;

        public UserService(IDbContextFactory<appDbContext> dbContextFactory,ProtectedSessionStorage sessionStorage )
        {
            _dbContextFactory = dbContextFactory;
            _sessionStorage = sessionStorage;
        }

        public List<User> GetAllUsers()
        {
            List<User> usersList = new List<User>();

            using (var context = _dbContextFactory.CreateDbContext())
            {
              var  users = context.Users.ToList();

                foreach (var u in users)
                {
                    User user = new User();
                    user = u;

                    usersList.Add(user);

                }
                return usersList;
            }
        }
    }
}
