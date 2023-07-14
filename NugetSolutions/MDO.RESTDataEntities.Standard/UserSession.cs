using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class UserSession
    {
        public int? UserSessionID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public bool IsMDOEmployee { get; set; }
        public bool IsMDOAdmin { get; set; }
    }
}
