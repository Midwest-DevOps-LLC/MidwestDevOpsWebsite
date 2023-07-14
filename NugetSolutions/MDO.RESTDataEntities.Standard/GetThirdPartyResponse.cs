using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class GetThirdPartyResponse
    {
        public List<ThirdPartyUser> ThirdPartyUsers { get; set; } = new List<ThirdPartyUser>();
    }
}
