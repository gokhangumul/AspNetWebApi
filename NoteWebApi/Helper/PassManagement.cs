using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NoteWepApi.Helper
{
    public class PassManagement
    {
        public PassManagement()
        {


        }
        public byte[] Hashing(string usermail)
        {
            byte[] hash = Encoding.ASCII.GetBytes(usermail);
            return hash;

        }


        public string HashPass(string pass, byte[] salt)
        {

            string pwd;
            Rfc2898DeriveBytes pbkdf = new Rfc2898DeriveBytes(pass, salt, 100000);
            byte[] dbkdf = pbkdf.GetBytes(32);
            pwd = Convert.ToBase64String(dbkdf);
            return pwd;

        }
    }
}