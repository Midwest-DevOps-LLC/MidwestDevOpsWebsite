using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class EndpointErrorResponse
    {
        public string StatusCode { get; set; }
        public string RequestId { get; set; }
        public string Path { get; set; }
    }
}
