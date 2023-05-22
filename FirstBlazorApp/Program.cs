using FirstBlazorApp.Auth;
using FirstBlazorApp.Data;
using FirstBlazorApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Azure") ?? 
    throw new NullReferenceException("No connection string");
// Add services to the container.
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//auth from session video
//https://www.youtube.com/watch?v=iq2btD9WufI&list=PLzewa6pjbr3IQEUfNiK2SROQC1NuKl6PV&index=12
//13:24

builder.Services.AddScoped<AuthProccedure>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthProccedure>();
// dbServices
builder.Services.AddDbContextFactory<appDbContext>((DbContextOptionsBuilder options)=> options.UseSqlServer(connectionString));
builder.Services.AddTransient<PostService>();
builder.Services.AddTransient<ForumService>();

builder.Services.AddSingleton<WeatherForecastService>();

// Auth services
builder.Services.AddSingleton<UserAccountService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseAuthentication();
app.UseAuthorization();

//IApplicationBuilder.(app);



app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
