using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketValidationServer.Dtos
{
    public class BarcodeServerDataRequest
    {
        public string ApplicationInstanceId { get; set; }

        public string TicketId { get; set; }
    }
}