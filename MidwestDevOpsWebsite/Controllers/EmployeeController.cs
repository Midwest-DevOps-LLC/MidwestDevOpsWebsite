using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidwestDevOpsWebsite.Controllers
{
    public class EmployeeController : BaseController
    {
        public EmployeeController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var employeeRequestor = new MDO.RESTServiceRequestor.Standard.EmployeeRequest(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));
            var employees = employeeRequestor.GetAll(true);

            if (employees.IsSessionValid == false)
            {
                return Unauthorized();
            }

            var model = new List<Models.EmployeeIndexModel>();

            foreach (var employee in employees.Data)
            {
                using (var userBLL = new BusinessLogicLayer.Users(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                {
                    var user = userBLL.GetUserByID(employee.UserID);

                    if (user != null)
                    {
                        var i = new Models.EmployeeIndexModel();

                        i.EmployeeData = employee;
                        i.Name = user.Username;

                        model.Add(i);
                    }
                }
            }

            return View(model);
        }

        public IActionResult View(int id)
        {
            var employeeRequestor = new MDO.RESTServiceRequestor.Standard.EmployeeRequest(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));
            var employees = employeeRequestor.Get(id);

            if (employees.IsSessionValid == false)
            {
                return Unauthorized();
            }

            var model = new Models.EmployeeViewModel();

            using (var userBLL = new BusinessLogicLayer.Users(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var user = userBLL.GetUserByID(employees.Data.UserID);

                if (user != null)
                {
                    model.Employee = employees.Data;
                    model.Name = user.Username;
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Models.EmployeeSaveModel model)
        {
            //Create entity
            var employee = new MDO.RESTDataEntities.Standard.Employee();
            employee.EmployeeID = model.EmployeeID;
            employee.Title = model.Title;
            employee.HireDate = model.HireDate.GetValueOrDefault();

            //Send the request
            var employeeRequestor = new MDO.RESTServiceRequestor.Standard.EmployeeRequest(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));
            var employeeResponse = employeeRequestor.Put(employee);

            return Json(employeeResponse.ValidationModel);
        }
    }
}
