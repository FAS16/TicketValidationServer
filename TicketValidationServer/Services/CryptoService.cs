using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace TicketValidationServer.Services
{
    public class CryptoService
    {
        // 256-bit symmetric key
        private readonly byte[] _secretKey = Encoding.ASCII.GetBytes("6B5970337336763979244226452948404D6351655468576D5A7134743777217A");
        private readonly string _rsaPrivateKeyPath = "C:\\Users\\Fahad Ali\\source\\repos\\TicketValidationServer\\TicketValidationServer\\Files\\rsa-key.pem";


        public byte[] GenerateHmac(byte[] message)
        {
            using (var hmac = new HMACSHA256(_secretKey))
            {
                return hmac.ComputeHash(message);
            }
        }

        public byte[] GenerateHmac(byte[] message, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(message);
            }
        }

        // Generate a sequence of random bytes for e.g. salting
        public byte[] GenerateRandomBytes(int byteSize)
        {
            SecureRandom random = new SecureRandom();
            byte[] bytes = new byte[byteSize];
            random.NextBytes(bytes);
            return bytes;
        }

        // Sign data using RSA private key
        public byte[] SignData(byte[] signData)
        {
            var privateKey = ReadPemFile(_rsaPrivateKeyPath);
            var signer = SignerUtilities.GetSigner("SHA256WITHRSA");
            signer.Init(true, privateKey);
            signer.BlockUpdate(signData, 0, signData.Length);
            var signature = signer.GenerateSignature();

            return signature;

        }

        private AsymmetricKeyParameter ReadPemFile(string pemFileName)
        {
            using (var fileStream = File.OpenText(pemFileName))
            {
                var keyPair = (AsymmetricCipherKeyPair)new PemReader(fileStream).ReadObject();
                return keyPair.Private;

            }
        }
    }
}