using FirstBlazorApp.Data;
using FirstBlazorApp.Models;
using ForumAdminPanel.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;


namespace FirstBlazorApp.Services
{
    public class ImageService
    {
        private IDbContextFactory<appDbContext> _dbContextFactory;

        public ImageService(IDbContextFactory<appDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        // Save Image to DB
        public Images SaveImage(Images imageImport)
        {
            using (var contex = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    if(imageImport != null)
                    {
                        Images image = new Images();
                        image.Title = imageImport.Title;
                        image.Content = imageImport.Content;
                        image.TimePostCreated = DateTime.UtcNow;
                        image.UserId = imageImport.UserId;

                       contex.Add(image);
                        contex.SaveChanges();

                        return image;
                    }
                    else { return null; }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        
        }

        // Call image 
        public async Task<Images> GetImage(int imgId)
        {
            using (var contex = _dbContextFactory.CreateDbContext())
            {
                try
                {   
                    Images image= new Images();
                    
                    bool doesImgExist= await contex.Images.Where(I=>I.Id==imgId).AnyAsync();   

                    if (doesImgExist)
                    {
                       image = await contex.Images.Where(I => I.Id == imgId).FirstOrDefaultAsync();

                        return image;
                    }
                    else { return null; }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
