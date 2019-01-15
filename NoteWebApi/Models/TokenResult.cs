using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWepApi.Models
{
    public class TokenResult
    {
        public string Token { get; set; }
        public string Token_Type { get; set; }
        public string Expires_in { get; set; }
    }
}