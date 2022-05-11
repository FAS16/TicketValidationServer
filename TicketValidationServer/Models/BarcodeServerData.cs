using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProtoBuf;

namespace TicketValidationServer.Models
{
    [ProtoContract]
    public class BarcodeServerData
    {
        [ProtoMember(1)]
        public byte[] ServerDataSerialized { get; set; }

        [ProtoMember(2)]
        public byte[] ServerDataSignature { get; set; }
    }
}