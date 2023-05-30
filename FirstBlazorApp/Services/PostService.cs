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

        // Get all posts
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

        // Get all posts async
        public async Task <List<Post>> GetAllPostsFromThemeAsync(int id)
        {
            var returnedPosts = new List<Post>();
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var recordedPosts = await  context.Posts.Where(P => P.ThemeId == id).ToListAsync();

                foreach (var post in recordedPosts)
                {
                    Post p = post;

                    returnedPosts.Add(p);
                }
            }
            return returnedPosts;
        }

        // Get post creator
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
        
        // Create new post
        public async Task<Post> CreateNewPost(Post inputPost, int userId, int themeId)
        {
            Post newPost = new Post();
            using(var context = _dbContextFactory.CreateDbContext())
            {
                if(inputPost != null && userId != null && themeId != null)
                {
                    newPost.Title= inputPost.Title;
                    newPost.Body = inputPost.Body;
                    newPost.Value = 0;
                    newPost.UserId = userId;
                    newPost.ThemeId = themeId;
                    newPost.TimePostCreated = DateTime.UtcNow;

                    context.Add(newPost);
                    context.SaveChanges();

                    return newPost;
                }
                else { return null; }
            }
        }
    }
}
