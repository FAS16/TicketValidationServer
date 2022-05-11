using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketValidationServer.Dtos;
using TicketValidationServer.Services;

namespace TicketValidationServer.Controllers
{
    // TODO: Uncomment when final version is done
    //[Authorize] 
    public class ClientController : ApiController
    {
        private readonly ClientService _clientService;

        public ClientController()
        {
            _clientService = new ClientService();
        }

        // Endpoint for mobile ticketing app to retrieve client-specific secret and barcode token
        [HttpPost]
        [Route("client/secret")]
        //[Authorize(Roles = "Passenger")] TODO: Uncomment when final version is done
        public IHttpActionResult GenerateClientSecret(ClientSecretRequest request)
        {
            var response = _clientService.GenerateClientSecret(request.ApplicationInstanceId);

            return Ok(response);
        }

        // Endpoint for mobile ticketing app to request signing of barcode server data
        [HttpPost]
        [Route("client/barcode-server-data")]
        //[Authorize(Roles = "Passenger")] TODO: Uncomment when final version is done
        public IHttpActionResult GetBarcodeServerData([FromBody] BarcodeServerDataRequest request)
        {
            var response = _clientService.GenerateBarcodeServerData(request.ApplicationInstanceId, request.TicketId);

            return Ok(response);
        }

        // Endpoint for validation app to get server public key in order to verify barcode server signature
        [HttpGet]
        [Route("client/server-public-key")]
        //[Authorize(Roles = "Inspector")] TODO: Uncomment when final version is done
        public IHttpActionResult GetServerPublicKey()
        {
            var serverPublicKey = _clientService.GetServerPublicKey();
            return Ok(serverPublicKey);
        }

        // Endpoint for validation app to get currently valid barcode keys
        [HttpGet]
        [Route("client/barcode-keys")]
        //[Authorize(Roles = "Inspector")] TODO: Uncomment when final version is done
        public IHttpActionResult GetBarcodeKeys()
        {
            var barcodeKeys = _clientService.GetBarcodeKeys();

            return Ok(barcodeKeys);
        }

        // Endpoint for validation app to get validation parameters 
        [HttpGet]
        [Route("client/barcode-configurations")]
        //[Authorize(Roles = "Inspector")] TODO: Uncomment when final version is done
        public IHttpActionResult GetBarcodeConfigs()
        {
            var barcodeConfigs = _clientService.GetBarcodeConfigs().ToList();

            var dto = new BarcodeConfigDto()
            {
                ServerSignatureLifetimeSeconds = barcodeConfigs[0].Value,
                ClientSignatureLifetimeSeconds = barcodeConfigs[1].Value,
                TokenSignatureLifetimeSeconds = barcodeConfigs[2].Value
            };

            return Ok(dto);
        }
    }
}
