using System.Security.Cryptography;

namespace TLSecure.Crypto
{
    public static class PBKDF2
    {
        public static byte[] DeriveKey(string password, byte[] salt, int iterations, int keyLength)
        {
            using Rfc2898DeriveBytes rfc2898DeriveBytes = new(password, salt, iterations);
            return rfc2898DeriveBytes.GetBytes(keyLength);
        }
    }
}