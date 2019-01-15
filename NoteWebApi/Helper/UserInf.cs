using NoteWepApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace NoteWepApi.Helper
{
    internal class UserInf
    {
        internal static int GetUser()
        {
            var claim = ClaimsPrincipal.Current.Identities.First().Claims;
            var result = Convert.ToInt32(claim.First(x => x.Type == "Sid").Value);
            return result;

        }
    }
}