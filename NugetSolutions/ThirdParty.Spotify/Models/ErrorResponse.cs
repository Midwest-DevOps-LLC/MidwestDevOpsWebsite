using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.ThirdParty.Spotify.Standard.Models
{
    public class ErrorResponse
    {
        public ErrorClass error { get; set; } = new ErrorClass();

        public class ErrorClass
        {
            public int status { get; set; }
            public string message { get; set; }
        }
    }
}
