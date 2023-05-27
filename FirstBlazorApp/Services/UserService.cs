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

        // Get all users async 
        public async Task<List<User>> GetAllUsersAsync()
        {
         
            using (var context = _dbContextFactory.CreateDbContext())
            {
                List<User> usersList= await context.Users.ToListAsync();
                
                return usersList;
            }
        }
        //TODO search result 
        //public async Task<List<User>> SearchUsers(string? userName, string? userEmail)
        //{
        //    using (var context = _dbContextFactory.CreateDbContextAsync())
        //    {
        //        List<User> users = await context.Result.Users
        //            .Where(U => U.UserName.Contains(userName).ord;
        //        return users;
        //    }
        //}

        public List<User> SearchUsers(string? userName, string? userEmail)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                List<User> users = context.Users
                    .Where(U => U.UserName.Contains(userName) || U.Email.Contains(userEmail) ).ToList();
                return users;
            }
        }

        //Get user by id
        public User GetUser(int userId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                bool doesProfileExist = context.Users.Any(UP => UP.Id == userId);

                if (doesProfileExist)
                {
                   User user= context.Users
                        .Where(UP => UP.Id == userId).FirstOrDefault();
                    return user;
                }
                else
                {
                    return null;
                }

            }
        }

        //Get users profile
        public UserProfile GetUserProfile(int userId)
        {   
            using (var context = _dbContextFactory.CreateDbContext())
            {   
                bool doesProfileExist = context.UserProfiles.Any(UP => UP.UserId == userId);

                if (doesProfileExist)
                {
                    UserProfile userProfile = context.UserProfiles
                        .Where(UP => UP.UserId == userId).FirstOrDefault();
                    return userProfile;
                }
                else
                {
                    return null;
                }    
            }
        }

        // Update user credentials
        public void UpdateUser(User inputUser)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                // User context
                if (inputUser != null)
                {
                    bool doesUserexist = context.Users.Any(U => U.Id == inputUser.Id);
                    if (doesUserexist)
                    {
                        User user = context.Users.Where(U => U.Id == inputUser.Id).FirstOrDefault();
                        user.UserName = inputUser.UserName;
                        user.Password = inputUser.Password;
                        user.Email = inputUser.Email;
                        user.UserStatus= inputUser.UserStatus;

                        context.SaveChanges();

                    }
                    else { context.Dispose(); }
                }
            }
        }

        // Update user profile
        public void UpdateUserProfile(UserProfile inputUserProfile)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                // User context
                if (inputUserProfile != null)
                {
                    bool doesUserexist = context.Users.Any(UP => UP.Id == inputUserProfile.UserId);
                    if (doesUserexist)
                    {
                        UserProfile userProfile = context.UserProfiles.Where(U => U.Id == inputUserProfile.UserId).FirstOrDefault();
                        userProfile.FirstName=  inputUserProfile.FirstName;
                        userProfile.LastName= inputUserProfile.LastName;
                        userProfile.Avatar= inputUserProfile.Avatar;
                        userProfile.AboutMyself = inputUserProfile.AboutMyself;

                        context.SaveChanges();
                    }
                    else { context.Dispose(); }
                }

            }
        }
        public void SaveUserProfileAndUser(User inputUser, UserProfile inputUserProfile)
        {
            using(var context=_dbContextFactory.CreateDbContext())
            {   
                // User context
                if(inputUser != null)
                {
                    bool doesUserexist = context.Users.Any(U => U.Id == inputUser.Id);
                    if(doesUserexist)
                    {
                        User user = context.Users.Where(U => U.Id == inputUser.Id).FirstOrDefault();
                        user.UserName=inputUser.UserName;
                        user.Password=inputUser.Password;
                        user.Email=inputUser.Email;

                        context.SaveChanges();

                    }
                    else {context.Dispose();}
                }
                // Profile context
                if(inputUserProfile != null)
                {
                    bool doesProfileExist = context.UserProfiles.Any(UP => UP.Id == inputUserProfile.Id);
                    if (doesProfileExist)
                    {
                        UserProfile userProfile = context.UserProfiles.Where(UP => UP.Id == inputUserProfile.Id).FirstOrDefault();
                        userProfile.FirstName=inputUserProfile.FirstName;

                        context.SaveChanges();
                    }
                    else {context.Dispose();}

                }  
            }
        }
    }
}
