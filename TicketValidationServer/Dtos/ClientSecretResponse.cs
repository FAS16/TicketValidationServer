
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketValidationServer.Dtos
{
    public class ClientSecretResponse
    {
        public byte[] Secret { get; set; }
        public string BarcodeToken { get; set; }
    }
}