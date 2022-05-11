using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProtoBuf;
using TicketValidationServer.Dtos;
using TicketValidationServer.Models;

namespace TicketValidationServer.Services
{
    public class ClientService
    {
        private readonly BarcodeKeyService _barcodeKeyService;
        private readonly CryptoService _cryptoService;
        private readonly ApplicationDbContext _database;

        public ClientService()
        {
            _barcodeKeyService = new BarcodeKeyService();
            _cryptoService = new CryptoService();
            _database = new ApplicationDbContext();
        }
        public ClientSecretResponse GenerateClientSecret(string applicationInstanceId)
        {
            var currentToken = _barcodeKeyService.GetCurrentToken();
            var appId = Encoding.ASCII.GetBytes(applicationInstanceId);

            var response = new ClientSecretResponse()
            {
                Secret = _cryptoService.GenerateHmac(appId, currentToken.KeyValue),
                BarcodeToken = currentToken.Id
            };

            return response;
        }

        public BarcodeServerDataResponse GenerateBarcodeServerData(string applicationInstanceId, string ticketId)
        {
            var serverData = new ServerData()
            {
                ApplicationInstanceId = applicationInstanceId,
                TicketId = ticketId,
                SignedAtUtc = ConvertToUnixTimeStamp(DateTime.UtcNow)
            };

            var serializedServerData = ProtoSerialize(serverData);
            var signature = _cryptoService.SignData(serializedServerData);

            var barcodeServerData = new BarcodeServerData
            {
                ServerDataSerialized = serializedServerData,
                ServerDataSignature = signature
            };

            return new BarcodeServerDataResponse(ProtoSerialize(barcodeServerData));
        }

        public string GetServerPublicKey()
        {
            const string rsaPublicKeyPath = "C:\\Users\\Fahad Ali\\source\\repos\\TicketValidationServer\\TicketValidationServer\\Files\\rsa-public-key.pem";
            return File.ReadAllText(rsaPublicKeyPath);
        }

        public IEnumerable<BarcodeKey> GetBarcodeKeys()
        {
            return _barcodeKeyService.GetBarcodeKeys();
        }

        public IEnumerable<BarcodeConfig> GetBarcodeConfigs()
        {
            return _database.BarcodeConfigurations.AsEnumerable();
        }

        private static byte[] ProtoSerialize<T>(T record) where T : class
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, record);
                return stream.ToArray();
            }

        }

        private static long ConvertToUnixTimeStamp(DateTime dateTime)
        {
            return (long)dateTime.Subtract(new DateTime(1970, 01, 01)).TotalSeconds;
        }

    }
}