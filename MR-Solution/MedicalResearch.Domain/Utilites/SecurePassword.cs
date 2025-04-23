using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace MedicalResearch.Domain.Utilites
{
    public static class SecurePassword
    {
        private const int SaltSize = 32;

        public static byte[] GenerateSalt()
        {
            var randomNumber = new byte[SaltSize];
            RandomNumberGenerator.Fill(randomNumber);
            return randomNumber;
        }

        public static byte[] ComputeHMAC_SHA256(byte[] data, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(data);
            }
        }
    }


}
