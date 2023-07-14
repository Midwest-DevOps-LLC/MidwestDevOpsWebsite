using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class RegisterRequest
    {
        public string Username
        {
            get; set;
        }

        public string Email
        {
            get; set;
        }

        public string Application
        {
            get; set;
        }

        public string Password
        {
            get; set;
        }

        public string RetypePassword
        {
            get; set;
        }
    }
}
