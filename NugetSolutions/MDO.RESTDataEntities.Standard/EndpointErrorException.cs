using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class EndpointErrorException : Exception
    {
        public EndpointErrorResponse EndpointErrorResponse { get; set; } = new EndpointErrorResponse();

        public EndpointErrorException(EndpointErrorResponse endpointErrorResponse)
        {
            this.EndpointErrorResponse = endpointErrorResponse;
        }
    }
}
