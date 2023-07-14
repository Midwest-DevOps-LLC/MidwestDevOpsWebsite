using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class DeleteUserPermissionRequest
    {
        public int UserID { get; set; }
        public int PermissionID { get; set; }
    }
}
