using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class ExternalApplication
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Permission> Permissions { get; set; } = new List<Permission>();

        public class Permission
        {
            public int? ExternalApplicationPermissionID { get; set; }
            public int ExternalApplicationID { get; set; }
            public int PermissionID { get; set; }
            public bool IsRequired { get; set; }
            public bool Active { get; set; }
        }
    }
}
