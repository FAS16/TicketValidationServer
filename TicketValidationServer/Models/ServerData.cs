using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProtoBuf;

namespace TicketValidationServer.Models
{
    [ProtoContract]
    public class ServerData
    {

        [ProtoMember(1)]
        public long SignedAtUtc { get; set; }

        [ProtoMember(2)]
        public string ApplicationInstanceId { get; set; }

        [ProtoMember(3)]
        public string TicketId { get; set; }
    }
}