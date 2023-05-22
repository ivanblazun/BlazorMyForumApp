using FirstBlazorApp.Data;
using ForumAdminPanel.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace FirstBlazorApp.Services
{
    public class ThemeService
    {
        private IDbContextFactory<appDbContext> _dbContextFactory;

        public ThemeService (IDbContextFactory<appDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public   List<Theme> GetAllThemesFromForum(int id)
        {       
            using (var context =_dbContextFactory.CreateDbContext())
            {   
                try
                {
                    bool selectedForum = context.Fora.Any(F=>F.Id==id);
                    if(selectedForum)
                    {
                        List<Theme> themeList = context.Themes.Where(T => T.ForumId == id).ToList();
                        return themeList;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("No themes",ex);
                }
            }

        }
    }
}
