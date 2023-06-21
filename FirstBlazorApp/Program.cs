using FirstBlazorApp.Auth;
using FirstBlazorApp.Data;
using FirstBlazorApp.Hubs;
using FirstBlazorApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Azure") ?? 
    throw new NullReferenceException("No connection string");
// Add services to the container.
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<AuthProccedure>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthProccedure>();

// dbServices
builder.Services.AddDbContextFactory<appDbContext>((DbContextOptionsBuilder options)=> options.UseSqlServer(connectionString));
builder.Services.AddTransient<PostService>();
builder.Services.AddTransient<ForumService>();
builder.Services.AddTransient<ThemeService>();
builder.Services.AddTransient<AnswerService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<ImageService>();
builder.Services.AddTransient<SqlService>();
builder.Services.AddSingleton<WeatherForecastService>();

//Email service
builder.Services.AddSingleton<EmailService>();

// Chat service
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

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

//Chat
app.MapHub<ChatHub>("/chathub");

app.MapFallbackToPage("/_Host");

app.Run();
