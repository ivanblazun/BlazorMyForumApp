using FirstBlazorApp.Data;
using FirstBlazorApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Azure") ?? 
    throw new NullReferenceException("No connection string");
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Auth services
builder.Services.AddAuthentication("Cookies").AddCookie();

builder.Services.AddSingleton<WeatherForecastService>();

// dbServices
builder.Services.AddDbContextFactory<appDbContext>((DbContextOptionsBuilder options)=> options.UseSqlServer(connectionString));

builder.Services.AddTransient<PostService>();


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
