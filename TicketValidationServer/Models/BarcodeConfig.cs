using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketValidationServer.Models
{
    public class BarcodeConfig
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }
    }
}