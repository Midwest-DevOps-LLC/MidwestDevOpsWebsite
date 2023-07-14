using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class Site
    {
        public int? SiteID { get; set; }
        public string GUID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool AllowAllUsers { get; set; }
        public string RegisterSuccessPath { get; set; }
        public string URL { get; set; }
    }
}
