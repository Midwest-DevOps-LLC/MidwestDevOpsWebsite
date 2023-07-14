using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class UserPermission
    {
        public int? UserPermissionID { get; set; }
        public int UserID { get; set; }
        public int PermissionID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Active { get; set; }
    }
}
