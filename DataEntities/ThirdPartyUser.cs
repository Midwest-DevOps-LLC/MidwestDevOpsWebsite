using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntities
{
    public class ThirdPartyUser
    {
        public int? ThirdPartyUserID { get; set; }
        public int ThirdPartyID { get; set; }
        public int UserID { get; set; }
        public string ApiKey { get; set; }
    }
}
