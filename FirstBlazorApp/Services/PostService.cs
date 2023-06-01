using FirstBlazorApp.Auth;
using FirstBlazorApp.Data;
using FirstBlazorApp.Pages.Posts;
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
            using (var context = _dbContextFactory.CreateDbContextAsync())
            {   

                var recordedPosts = await  context.Result.Posts.Where(P => P.ThemeId == id).ToListAsync();

                foreach (var post in recordedPosts)
                {   
                    if(post != null)
                    {
                        Post p = post;
                        returnedPosts.Add(p);
                    }

                }
            }
            return returnedPosts;
        }

        // Get single post 
        public async Task<Post> GetSinglePostAsync(int postId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                bool doesPostexist = context.Posts.Any(P => P.Id == postId);

                if (doesPostexist && postId != null)
                {
                    Post post = await context.Posts.FirstOrDefaultAsync(P => P.Id == postId);

                    return post;

                }
                else
                {
                    return null;
                }
            }
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
        
        // Update post 
        public async Task<Post> UpadetPostAsync(Post inputPost, int postId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                bool doesPostexist = context.Posts.Any(P => P.Id == postId);

                if (doesPostexist && postId != null)
                {
                    Post post = await context.Posts.FirstOrDefaultAsync(P => P.Id == postId);

                    post.Title= inputPost.Title;
                    post.Body = inputPost.Body;
                    post.TimePostCreated= DateTime.UtcNow;
                    post.Value = 0;

                    context.SaveChanges();

                    return post;
                }
                else
                {
                    return null;
                }
            }
        }

        // Delete post
        public async Task DeletePostAsync(Post deletedPost)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {   
                bool doesPostExist = await context.Posts.AnyAsync(P => P.Id == deletedPost.Id);

                if (doesPostExist && deletedPost != null) 
                {   
                    Post post = context.Posts.FirstOrDefault(p => p.Id == deletedPost.Id);

                    context.Posts.Remove(post);
                    context.SaveChanges();
                }
                
            }
        }
    }   
}
