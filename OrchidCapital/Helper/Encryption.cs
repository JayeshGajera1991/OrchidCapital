using System.Security.Cryptography;
using System.Text;

namespace OrchidCapital.Helper
{
    public static class Encryption
    {
        public static Random random = new Random();
        private static readonly string Key = "12345678901234567890123456789012";
        private static readonly string IV = "1234567890123456";
        public static string Encrypt(string encryptionKey, string clearText)
        {
            try
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch
            {
                clearText = string.Empty;
            }
            return clearText;
        }

        public static string Decrypt(string decryptionKey, string cipherText)
        {
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(decryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }

            }
            catch
            {
                cipherText = string.Empty;
            }
            return cipherText;
        }

        public static string PasswordGenerator()
        {
            int length = 8;
            var specialLength = (int)Math.Ceiling(length / 4d);
            var lowerUpperLength = (int)Math.Ceiling(length / 3d);
            var numericLength = (int)Math.Ceiling(length / 2d);

            var special = Enumerable.Repeat("!@#$%^&|+-.,?", specialLength)
                .Select(chars => chars[random.Next(chars.Length)]).ToArray();
            var lower = Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz", lowerUpperLength)
                .Select(chars => chars[random.Next(chars.Length)]).ToArray();
            var upper = Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", lowerUpperLength)
                .Select(chars => chars[random.Next(chars.Length)]).ToArray();
            var numeric = Enumerable.Repeat("0123456789", numericLength)
                .Select(chars => chars[random.Next(chars.Length)]).ToArray();

            var scrambledConcat = special.Concat(lower)
                .Concat(upper).Concat(numeric)
                .ToArray();

            var scrambledChars = Enumerable.Repeat(scrambledConcat, length)
                .Select(chars => chars[random.Next(chars.Length)]).ToArray();

            return new string(scrambledChars);
        }

        public static string DecryptParameter(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(IV);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
