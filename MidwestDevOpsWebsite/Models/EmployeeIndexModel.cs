using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidwestDevOpsWebsite.Models
{
    public class EmployeeIndexModel
    {
        public MDO.RESTDataEntities.Standard.Employee EmployeeData { get; set; } = new MDO.RESTDataEntities.Standard.Employee();
        public string Name { get; set; }
    }
}
