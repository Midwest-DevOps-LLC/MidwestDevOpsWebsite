using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class LoginResponse
    {
        public User User { get; set; } = new User();
        public string Auth { get; set; }
    }
}
