using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RecruitingChallenge.Common.Helpers
{
    public static class Hasher
    {
        public static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            string pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return pass;
        }
    }
}
