using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntities
{
    public class ThirdParty
    {
        public int? ThirdPartyID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public string AuthorizeUrl { get; set; }
        public string Description { get; set; }
    }
}
