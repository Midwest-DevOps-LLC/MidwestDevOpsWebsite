using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.ThirdParty.Spotify.Standard.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
    }
}
