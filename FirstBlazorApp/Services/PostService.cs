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
     
    

        public PostService (IDbContextFactory<appDbContext> dbContextFactory, CustomAuthService auth)
        {
        
            
            _dbContextFactory = dbContextFactory;
        }


        //public void AddPost

        public List<Post> GetAllPosts()  
        {


            var returnedPosts = new List<Post>();

            using ( var context = _dbContextFactory.CreateDbContext()) 
            {
                var recordedPosts= context.Posts.ToList();

                foreach ( var post in recordedPosts) 
                {
                    Post p = post;

                    returnedPosts.Add( p );
                }
            }

                return returnedPosts;
        } 
    }
}
