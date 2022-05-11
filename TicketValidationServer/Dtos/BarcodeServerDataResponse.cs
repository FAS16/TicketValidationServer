using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketValidationServer.Dtos
{
    public class BarcodeServerDataResponse
    {
        public byte[] BarcodeServerData { get; set; }
        public BarcodeServerDataResponse(byte[] barcodeServerData)
        {
            BarcodeServerData = barcodeServerData;
        }
    }
}