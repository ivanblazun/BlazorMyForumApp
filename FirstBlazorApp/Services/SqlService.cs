using FirstBlazorApp.Data;
using FirstBlazorApp.Pages.Users;
using ForumAdminPanel.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing;

namespace FirstBlazorApp.Services
{
    public class SqlService
    {
        private IDbContextFactory<appDbContext> _dbContextFactory;

        private readonly IConfiguration _configuration;


        public SqlService(IDbContextFactory<appDbContext> dbContextFactory,IConfiguration configuration)
        {
            _dbContextFactory = dbContextFactory;
            _configuration = configuration;
        }

        public class UserRelatedContent
        {
            public  List<Fora> Foras { get; set; }
            public List<Theme>? Themes { get; set; }
            public List<Post>? Posts { get; set; }

        }

        public async Task<UserRelatedContent> GetUserCreatedContentAsync(User user)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                if (user.Id == null) { return null; }
                else 
                { 
                    UserRelatedContent userRelatedContent = new UserRelatedContent();

                    userRelatedContent.Foras = await context.Fora.Where(F => F.UserId == user.Id).ToListAsync() ?? null;
                    userRelatedContent.Themes = await context.Themes.Where(T => T.UserId == user.Id).ToListAsync() ?? null;
                    userRelatedContent.Posts = await context.Posts.Where(P => P.UserId == user.Id).ToListAsync() ?? null;

                    return userRelatedContent;
                }
            }
        }


        // Test sql query
        public async Task<UserRelatedContent> GetUserCreatedContentSqlQ(User user)
        {   
            UserRelatedContent userRelatedContent = new UserRelatedContent();

            string cs = _configuration.GetConnectionString("Azure");
 
            if (user.Id != null && user.UserName != null)
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    SqlCommand sqlCommand = new SqlCommand(
                            $"SELECT * FROM dbo.Fora WHERE Fora.UserId = {user.Id}",
                            conn
                        );
                    SqlDataReader reader;
                    reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Fora fora = new Fora();
                        fora.Id = reader.GetInt32("Id");
                        fora.Name = reader.GetString("Name");
                        fora.ThemesCounter = reader.GetInt32("ThemesCounter");
                        fora.UserCounter = reader.GetInt32("UserCounter");
                        fora.MainForumId = reader.GetInt32("MainForumId");
                        fora.Description = reader.GetString("Description");
                        fora.UserId = reader.GetInt32("UserId");
                        fora.TimeForumCreated = reader.GetDateTime("TimeForumCreated");
                        fora.Themes = new List<Theme>() { };
                         
                        userRelatedContent.Foras.Add(fora);
                    };
                                       
                    return userRelatedContent;

                    conn.Close();

                };
            }
            else { return null; }
                      

            //using (var context = _dbContextFactory.CreateDbContext())
            //{
            //    UserRelatedContent userRelatedContent = new UserRelatedContent();

            //    userRelatedContent.Foras = from forums in context.Fora select forums where 
            //}
            //return null;
        }
    }
}
