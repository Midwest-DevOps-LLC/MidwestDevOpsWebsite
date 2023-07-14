using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidwestDevOpsWebsite.Models
{
    public class EmployeeSaveModel
    {
        public int? EmployeeID { get; set; }
        public DateTime? HireDate { get; set; }
        public string Title { get; set; }
    }
}
