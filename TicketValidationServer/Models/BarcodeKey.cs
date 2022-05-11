using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketValidationServer.Models
{
    public class BarcodeKey
    {
        public string Id { get; set; }

        public byte[] KeyValue { get; set; }

        public long ValidFromHour { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}