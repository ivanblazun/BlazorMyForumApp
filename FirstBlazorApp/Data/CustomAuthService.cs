using System.Security.Claims;

namespace FirstBlazorApp.Data
{
    public class CustomAuthService
    {   
        public Dictionary<string,ClaimsPrincipal>Users { get; set; }
        public CustomAuthService() 
        {
            Users=new Dictionary<string, ClaimsPrincipal> ();
        }

    }
}
