using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class Employee
    {
        public int? EmployeeID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public bool IsAdmin { get; set; }
        public bool Active { get; set; }
        public DateTime HireDate { get; set; }
    }
}
