using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk
{
    internal class MojeHaslo : PasswordHasher
    {
        public override string HashPassword(string password)
        {
            return password;
            //return Crypto.Sha1.Encrypt(password);
        }

        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            //var testHash = Crypto.Sha1.Encrypt(providedPassword);
            return hashedPassword.Equals("testHash") || hashedPassword.Equals(providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}