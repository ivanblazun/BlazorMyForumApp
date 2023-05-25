using FirstBlazorApp.Data;
using ForumAdminPanel.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace FirstBlazorApp.Auth
{
    public class AuthProccedure : AuthenticationStateProvider
    {   
        private readonly ProtectedSessionStorage _sessionStorage;

        private IDbContextFactory<appDbContext> _dbContextFactory;

        private readonly ClaimsPrincipal _anonymus = new ClaimsPrincipal(new ClaimsIdentity());

        public AuthProccedure(ProtectedSessionStorage sessionStorage, IDbContextFactory<appDbContext> dbContextFactory) 
        {
            _sessionStorage = sessionStorage;    
            _dbContextFactory = dbContextFactory;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymus));
                }
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                    new List<Claim>
                    {
                    new Claim(ClaimTypes.Name,userSession.UserName),
                    new Claim(ClaimTypes.Role,userSession.UserStatus)
                    }, "CustomAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch 
            {

                return await Task.FromResult(new AuthenticationState(_anonymus));
            }
        }

        public async Task<User> GetCurrentUserAsync()
        {   
            try
            {
                var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
                if( userSessionStorageResult.Success && userSession.UserName !=null)
                {
                    var currentUserName = userSession.UserName;

                    using (var context = _dbContextFactory.CreateDbContext())
                    {   
                        User currentUser = context.Users.FirstOrDefault(U=>U.UserName==currentUserName);
                        return currentUser;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpadateAuthState(UserSession userSession) 
        {
            ClaimsPrincipal claimsPrincipal;

            if(userSession != null)
            {
                await _sessionStorage.SetAsync("UserSession", userSession);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                   new List<Claim>
                   {
                    new Claim(ClaimTypes.Name,userSession.UserName),
                    new Claim(ClaimTypes.Role,userSession.UserStatus)
                   }));
            }
            else
            {
                await _sessionStorage.DeleteAsync("UserSession");
                claimsPrincipal = _anonymus;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
