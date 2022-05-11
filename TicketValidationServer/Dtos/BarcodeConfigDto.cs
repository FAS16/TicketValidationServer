using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketValidationServer.Dtos
{
    public class BarcodeConfigDto
    {
        public int ServerSignatureLifetimeSeconds { get; set; }
        public int ClientSignatureLifetimeSeconds { get; set; }
        public int TokenSignatureLifetimeSeconds { get; set; }
    }
}