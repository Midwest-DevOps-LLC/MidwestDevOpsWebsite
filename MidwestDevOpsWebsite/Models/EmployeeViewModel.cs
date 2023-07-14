using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidwestDevOpsWebsite.Models
{
    public class EmployeeViewModel
    {
        public MDO.RESTDataEntities.Standard.Employee Employee { get; set; } = new MDO.RESTDataEntities.Standard.Employee();
        public string Name { get; set; }
    }
}
