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

        // Get all themes
        public async  Task<List<Theme>> GetAllThemesFromForumAsync(int id)
        {       
            using (var context =_dbContextFactory.CreateDbContext())
            {   
                try
                {
                    bool selectedForum = await context.Fora.AnyAsync(F=>F.Id==id);
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

        // Search for theme
        public List<Theme> SearchThemes(string? themeTitle, string? themeBody)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                List<Theme> themes = context.Themes
                    .Where(T => T.Title.Contains(themeTitle) || T.ThemeBody.Contains(themeBody)).ToList();
                return themes;
            }
        }

        // Add new theme
        public async Task<Theme> CreateNewTheme(Theme themeInput, int userId, int forumId)
        {
            Theme theme= new Theme();
            using(var context = _dbContextFactory.CreateDbContext())
            {
                if (themeInput != null && userId != null && forumId != null)
                {
                    theme.Title = themeInput.Title;
                    theme.Value = 0;
                    theme.UserId = userId;
                    theme.ForumId = forumId;
                    theme.TimeThemeCreated = DateTime.UtcNow;
                    theme.ThemeBody = themeInput.ThemeBody;

                    context.Add(theme);
                    context.SaveChanges();
                    return theme;
                }
                else { return null; }
            }
        }

        // Get Theme by id
        public async Task<Theme> GetThemeAsync(int themeId)
        {
            Theme theme = new Theme();
            using (var context = _dbContextFactory.CreateDbContext())
            {
                if (themeId != null) 
                {
                    theme = await context.Themes.FirstOrDefaultAsync(T => T.Id == themeId);
                    return theme;
                }
                else { return null; }
            }
        }
        // Get Theme by title
        public Theme GetThemeByTitle(string themeTitle)
        {
            Theme theme = new Theme();
            using (var context = _dbContextFactory.CreateDbContext())
            {
                if (themeTitle != null)
                {
                    theme = context.Themes.FirstOrDefault(T => T.Title == themeTitle);
                    return theme;
                }
                else { return null; }
            }
        }

        // Update theme
        public async Task<Theme> UpdateThemeAsync(Theme inputTheme,int themeId)
        {
            using(var context = _dbContextFactory.CreateDbContext())
            {   
                if(themeId != null)
                {
                    bool doesThemeExist=await  context.Themes.AnyAsync(T => T.Id == themeId);

                    if(doesThemeExist)
                    {
                        Theme theme = await context.Themes.FirstOrDefaultAsync(T => T.Id == themeId);

                        theme.Title= inputTheme.Title;
                        theme.Value = 0;
                        theme.ThemeBody = inputTheme.ThemeBody;
                        theme.TimeThemeCreated = DateTime.UtcNow;
                        theme.ForumId= inputTheme.ForumId;

                        context.SaveChanges();

                        return theme;

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        // Delete them 
        public async Task DeleteTheme(int themeId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    if(themeId != null &&  await context.Themes.AnyAsync(t => t.Id == themeId) == true)
                    {
                        Theme theme= await context.Themes.FirstOrDefaultAsync(t => t.Id == themeId);

                        context.Themes.Remove(theme);
                        context.SaveChanges();
                    }

                }
                catch (Exception)
                {

                    throw;
                }
        
            }
        }

    }
}
