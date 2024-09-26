using System.Security.Cryptography;

namespace TLSecure.Crypto
{
    public static class HMAC
    {
        public static byte[] Sign(byte[] data, byte[] key)
        {
            using (HMACSHA256 hmacsha = new(key))
            {
                return hmacsha.ComputeHash(data);
            }
        }
    }
}