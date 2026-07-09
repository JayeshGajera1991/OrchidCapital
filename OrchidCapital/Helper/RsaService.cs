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
            var helperFolder = Path.Combine(AppContext.BaseDirectory, "Helper");

            var publicKeyPath = Path.Combine(helperFolder, "public.pem");
            var privateKeyPath = Path.Combine(helperFolder, "private.pem");

            _publicKey = File.ReadAllText(publicKeyPath);
            _privateKey = File.ReadAllText(privateKeyPath);
        }

        public string GetPublicKey()
        {
            var publicKeyPath = Path.Combine(
                AppContext.BaseDirectory,
                "Helper",
                "public.pem"
            );
            return System.IO.File.ReadAllText(publicKeyPath);
        }

        public string Encrypt(string text)
        {
            try
            {
                using RSA rsa = RSA.Create();
                rsa.ImportFromPem(_publicKey);
                byte[] data = Encoding.UTF8.GetBytes(text);
                byte[] encryptedBytes = rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedBytes); ;
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
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
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