﻿using FirstBlazorApp.Data;
using FirstBlazorApp.Models;
using ForumAdminPanel.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace FirstBlazorApp.Services
{
    public class ImageService
    {
        private IDbContextFactory<appDbContext> _dbContextFactory;

        public ImageService(IDbContextFactory<appDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        
        ///////////////////////////////////////////////////
        // Save Image of user to DB 
        public Images SaveImage(Images imageImport,User currentUser)
        {
            using (var contex = _dbContextFactory.CreateDbContext())
            {
                bool doesImageAlredyExist = contex.Images.Any(I => I.UserId == currentUser.Id);
                bool doesImageHasThemeId = contex.Images.Any(I => I.ThemeId == null);

                if (doesImageAlredyExist && doesImageHasThemeId) 
                {
                    Images existingImage = contex.Images.FirstOrDefault(I => I.UserId == currentUser.Id);
                    contex.Images.Remove(existingImage);
                    contex.SaveChanges();
                }
                try
                {
                    if(imageImport != null)
                    {
                        Images image = new Images();
                        image.Title = imageImport.Title;
                        image.Content = imageImport.Content;
                        image.TimePostCreated = DateTime.UtcNow;
                        image.UserId = currentUser.Id;
                        //image.ThemeId = 9;

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

        // Save Image of theme to DB 
        public Images SaveImage(Images imageImport, User currentUser,Theme theme)
        {
            using (var contex = _dbContextFactory.CreateDbContext())
            {
                // bool doesImageAlredyExist = contex.Images.Any(I => I.UserId == currentUser.Id);
                bool doesImageHasUserId = contex.Images.Any(I => I.UserId == null);
                bool doesImageHasThemeId = contex.Images.Any(I=>I.ThemeId ==  theme.Id);
                if (doesImageHasUserId && doesImageHasThemeId)
                {
                    Images existingImage = contex.Images.FirstOrDefault(I => I.ThemeId == theme.Id);
                    contex.Images.Remove(existingImage);
                    contex.SaveChanges();
                }
                else
                {

                    try
                    {
                        if (imageImport != null)
                        {
                            Images image = new Images();
                            image.Title = imageImport.Title;
                            image.Content = imageImport.Content;
                            image.TimePostCreated = DateTime.UtcNow;
                            image.ThemeId = theme.Id;

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
                    return null;
            }

        }

        // For User Images 
        public async Task SaveImagetoDatabase(InputFileChangeEventArgs e,User user)
        {

            Images Img = new Images();

            using MemoryStream ms = new();
            var resized = await e.File.RequestImageFileAsync(e.File.ContentType, maxWidth: 500, maxHeight: 500);
            using Stream fileStream = resized.OpenReadStream();
            await fileStream.CopyToAsync(ms);

            Img.Content = ms.ToArray();
            Img.Title = e.File.Name;
            Img.UserId = user.Id;

            SaveImage(Img,user);
                              

        }

        // For Theme Images
        public async Task SaveImagetoDatabase(InputFileChangeEventArgs e, User user,Theme theme)
        {
            try
            {
                Images Img = new Images();

                using MemoryStream ms = new();
                var resized = await e.File.RequestImageFileAsync(e.File.ContentType, maxWidth: 500, maxHeight: 500);
                using Stream fileStream = resized.OpenReadStream();
                await fileStream.CopyToAsync(ms);

                Img.Content = ms.ToArray();
                Img.Title = e.File.Name;
                Img.ThemeId= theme.Id;

                SaveImage(Img, user,theme);

            }
            catch (Exception q)
            {

                throw q;
            }


        }




        ///////////////////////////////////////////////////
        // Call image and convert from sql to image string,
        // Returns "path" to string (string imageSrc = returned string)

        // Get image for User
        public async Task<string> GetImage(User user)
        {
            string imageSrc = "";

            using (var contex = _dbContextFactory.CreateDbContext())
            {
                try
                {   
                    Images image= new Images();
                    
                    bool doesImgExist= await contex.Images.Where(I=>I.UserId==user.Id).AnyAsync();   

                    if (doesImgExist)
                    {
                       image = await contex.Images.Where(I => I.UserId == user.Id).FirstOrDefaultAsync();


                       return  ConvertFromSqlToImage(image,imageSrc);
                                            
                    }
                    else { return null; }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        // get image for theme
        public async Task<string> GetImage(Theme theme)
        {
            string imageSrc = "";

            using (var contex = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    Images image = new Images();

                    bool doesImgExist = await contex.Images.Where(I => I.ThemeId == theme.Id).AnyAsync();

                    if (doesImgExist)
                    {
                        image = await contex.Images.Where(I => I.ThemeId == theme.Id).FirstOrDefaultAsync();


                        return ConvertFromSqlToImage(image, imageSrc);

                    }
                    else { return null; }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        //Image from sql converter 
        public string ConvertFromSqlToImage(Images upImage, string imageSrc)
        {
            Image IS;
            try
            {

                // set data to image content
                Byte[] data = new Byte[0];
                data = (Byte[])upImage.Content;


                MemoryStream ms = new MemoryStream(data);
                IS = Image.FromStream(ms);

                // convert image content to image string
                ImageConverter ic = new ImageConverter();
                byte[] thumbBytes = (byte[])ic.ConvertTo(IS, typeof(byte[]));
                string img64 = Convert.ToBase64String(thumbBytes);
                imageSrc = string.Format("data:image/jpg;base64, {0}", img64);
                return imageSrc;

            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
