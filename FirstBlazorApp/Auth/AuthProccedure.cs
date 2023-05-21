using System.Security.Principal;

namespace FirstBlazorApp.Auth
{
    public class AuthProccedure 
    {
        public IIdentity CheckUserIdentity()
        {

            ConetxtHubs conetxtHubs = new ConetxtHubs();

            var currUser = conetxtHubs.Context.User.Identity;

            return currUser;

        }
    }
}
