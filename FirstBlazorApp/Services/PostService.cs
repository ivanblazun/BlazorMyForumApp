using FirstBlazorApp.Auth;
using FirstBlazorApp.Data;
using ForumAdminPanel.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FirstBlazorApp.Services
{
    public class PostService
    {
        private IDbContextFactory<appDbContext> _dbContextFactory;
     
    

        public PostService (IDbContextFactory<appDbContext> dbContextFactory)
        {         
            _dbContextFactory = dbContextFactory;
        }

        public List<Post> GetAllPostsFromTheme(int id)  
        {
            var returnedPosts = new List<Post>();
            using ( var context = _dbContextFactory.CreateDbContext()) 
            {   
                var recordedPosts= context.Posts.Where(P=>P.ThemeId==id).ToList();

                foreach ( var post in recordedPosts) 
                {
                    Post p = post;

                    returnedPosts.Add( p );
                }
            }
                return returnedPosts;
        }

        public string GetUserCreatorOfPost(int userId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                User user = new User();
                user = context.Users.Where(U => U.Id == userId).FirstOrDefault();
                if (user != null)
                {
                    return user.UserName.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
        //public async Task<string> GetUserCreatorOfPost(int userId)
        //{
        //    using (var context = _dbContextFactory.CreateDbContext())
        //    {
        //        User user = new User();
        //        user = await context.Users.Where(U => U.Id == userId).FirstOrDefaultAsync();
        //        if (user != null)
        //        {
        //            return user.UserName.ToString();
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
    }
}
