using System.Security.Cryptography;
using System.Text;

namespace OrchidCapital.Helper
{
    public class RsaService
    {
        private readonly string _publicKey;
        private readonly string _privateKey;
        private static readonly RSA _rsa = RSA.Create(2048);
        public RsaService(IConfiguration config)
        {
            _publicKey = System.IO.File.ReadAllText("Helper/public.pem");
            _privateKey = System.IO.File.ReadAllText("Helper/private.pem");
        }

        public string GetPublicKey()
        {
            return System.IO.File.ReadAllText("Helper/public.pem");
        }

        public string Encrypt(string text)
        {
            try
            {
                using RSA rsa = RSA.Create();
                rsa.ImportFromPem(_publicKey);
                byte[] data = Encoding.UTF8.GetBytes(text);
                byte[] encryptedBytes = rsa.Encrypt(data,RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedBytes);;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Encryption failed: {ex.Message}");
                return null;
            }
        }

        public string Decrypt(string encryptedText)
        {
            try
            {
                encryptedText = encryptedText.Replace(" ", "+");
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                using RSA rsa = RSA.Create();
                rsa.ImportFromPem(_privateKey);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes,RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Decryption failed: {ex.Message}");
                return null; // Return null or an appropriate value to indicate failure
            }
        }
    }
}