using FirstBlazorApp.Data;
using ForumAdminPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstBlazorApp.Services
{
    public class ForumService
    {
        private IDbContextFactory<appDbContext> _dbContextFactory;

        public ForumService(IDbContextFactory<appDbContext> dbContextFactory) 
        {
            _dbContextFactory = dbContextFactory;
        }


        public async Task<List<Fora>> GetAllForumsAsync()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var forums = context.Fora.ToListAsync();
                return await forums;
            }
        }

        public List<Fora> GetAllForums()
        {   
            using(var context = _dbContextFactory.CreateDbContext())
            {
                var forums= context.Fora.ToList();
                return forums;
            }
        }

        public Fora GetForum(int id) 
        {
            using (var contex = _dbContextFactory.CreateDbContext()) 
            {   
                var forumById= contex.Fora.FirstOrDefault(F=>F.Id==id);
                bool doesForumExist= contex.Fora.Any(F=>F.Id==id);
                
                if (doesForumExist) 
                {
                    return forumById;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
