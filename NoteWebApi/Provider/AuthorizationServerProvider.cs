using Microsoft.Owin.Security.OAuth;
using NoteWepApi.Helper;
using NoteWepApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace NoteWepApi.Provider
{
    public class AuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
           context.Validated();

        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                PassManagement pm = new PassManagement();
                byte[] salt = pm.Hashing(context.UserName);
                string hashing = pm.HashPass(context.Password, salt);
                var user = db.USERS.Where(x => x.Mail == context.UserName && x.Hash == hashing).
                    Select(x => new { x.Id, x.Name, x.Mail, x.RegisterDate, x.UserName, x.UserImage }).ToList();
                if (user.Count==0)
                {
                     context.SetError("oturum_hatası", "Mail adresi veya şifre hatalı.");
                }
                else
                {
                    var user1 = user.FirstOrDefault();
                    var idendity = new ClaimsIdentity(context.Options.AuthenticationType);
                    idendity.AddClaim(new Claim("Sid", Convert.ToString(user1.Id)));
                    context.Validated(idendity);
                }

            }
        

        }
    }
}