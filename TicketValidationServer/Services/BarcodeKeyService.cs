using System;
using System.Collections.Generic;
using System.Linq;
using TicketValidationServer.Models;

namespace TicketValidationServer.Services
{
    public class BarcodeKeyService
    {
        private readonly ApplicationDbContext _database;
        private readonly CryptoService _cryptoService;

        public BarcodeKeyService()
        {
            _database = new ApplicationDbContext();
            _cryptoService = new CryptoService();
        }

        // Gets the token that is associated with the current hour of the day
        public BarcodeKey GetCurrentToken()
        {

            var hoursSinceEpoch = GetHoursSinceEpoch();

            var token = _database.BarcodeKeys.SingleOrDefault(t => t.ValidFromHour == hoursSinceEpoch);

            if (token != null)
            {
                return token;
            }

            // No valid barcodeKey in database, generate new tokens and add to database
            var newTokens = GenerateNewBarcodeKeys();

            // The first barcodeKey in the range of new tokens is the currently valid barcodeKey
            return newTokens[0];

        }

        public List<BarcodeKey> GenerateNewBarcodeKeys()
        {
            var hoursSinceEpoch = GetHoursSinceEpoch();
            var nowUtc = DateTime.UtcNow;
            var newTokens = new List<BarcodeKey>();
            for (int i = 0; i < 48; i++) // Should ideally be configurable
            {
                var randomBytes = _cryptoService.GenerateRandomBytes(32);
                var key = _cryptoService.GenerateHmac(randomBytes);

                var t = new BarcodeKey()
                {
                    Id = Guid.NewGuid().ToString(),
                    KeyValue = key,
                    ValidFromHour = hoursSinceEpoch + i,
                    CreatedAt = nowUtc
                };

                newTokens.Add(t);
            }

            _database.BarcodeKeys.AddRange(newTokens);
            _database.SaveChanges();

            return newTokens;

        }

        public IEnumerable<BarcodeKey> GetBarcodeKeys()
        {
            var hoursSinceEpoch = GetHoursSinceEpoch();
            // Setting limit to get barcode keys from the previous and next 24 hours
            var lowerLimit = hoursSinceEpoch - 24;
            var upperLimit = hoursSinceEpoch + 24;

            var validBarcodeKeys = _database.BarcodeKeys.Where(barcodeKey => barcodeKey.ValidFromHour > lowerLimit && barcodeKey.ValidFromHour < upperLimit)
                .ToList();

            if (!validBarcodeKeys.Any())
            {
                validBarcodeKeys = GenerateNewBarcodeKeys();
            }

            return validBarcodeKeys;

        }

        private static int GetHoursSinceEpoch()
        {
            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 01, 01)).TotalHours;
        }

    }
}