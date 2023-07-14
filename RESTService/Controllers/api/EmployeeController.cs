using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : BaseController
    {
        //public EmployeeController(IConfiguration configuration) : base(configuration)
        //{
        //    _configuration = configuration;
        //}

        /// <summary>
        /// Gets an employee by employee id. Perm: Employee.GetEmployeeInformation
        /// </summary>
        [HttpGet]
        public IActionResult Get(int employeeID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.Employee>();

            var sessionResponse = OnlyPermission(response, MDO.RESTDataEntities.Standard.Permission.PermissionLookup.Employee.GetEmployeeInformation);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            var session = GetSession;

            using (var siteBAL = new RESTBLL.Employees(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                response.Data = siteBAL.GetEmployeeByID(employeeID);
            }

            return Json(response);
        }

        /// <summary>
        /// Get all employees. Perm: Employee.View
        /// </summary>
        [HttpGet("GetAll")]
        public IActionResult GetAll(bool? active)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Employee>>();

            var sessionResponse = OnlyPermission(response, MDO.RESTDataEntities.Standard.Permission.PermissionLookup.Employee.View);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            var session = GetSession;

            using (var siteBAL = new RESTBLL.Employees(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                response.Data = siteBAL.GetAllEmployees(active);
            }

            return Json(response);
        }

        /// <summary>
        /// Update or creates an employee. Returns the EmployeeID. Perm: Employee.CreateUpdate
        /// </summary>
        [HttpPut]
        public IActionResult Put(MDO.RESTDataEntities.Standard.Employee employee)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>("Couldn't update employee");

            var sessionResponse = OnlyPermission(response, MDO.RESTDataEntities.Standard.Permission.PermissionLookup.Employee.CreateUpdate);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            //Validation
            if (string.IsNullOrEmpty(employee.Title))
                response.ValidationModel.Add(new MDO.RESTDataEntities.Standard.ValidationMessage("title", "Title cannot be empty", MDO.RESTDataEntities.Standard.ValidationStatus.Error));

            if (employee.HireDate == DateTime.MinValue)
                response.ValidationModel.Add(new MDO.RESTDataEntities.Standard.ValidationMessage("hireDate", "Hire Date cannot be empty", MDO.RESTDataEntities.Standard.ValidationStatus.Error));

            if (response.ValidationModel.ValidationStatus == MDO.RESTDataEntities.Standard.ValidationStatus.Error)
            {
                return Json(response);
            }

            var session = GetSession;

            using (var siteBAL = new RESTBLL.Employees(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var foundEmployee = siteBAL.GetEmployeeByID(employee.EmployeeID.GetValueOrDefault());

                long? saveID = null;

                if (foundEmployee != null)
                {
                    foundEmployee.Title = employee.Title;
                    foundEmployee.HireDate = employee.HireDate;

                    saveID = siteBAL.SaveEmployee(foundEmployee);
                }
                else
                {
                    saveID = siteBAL.SaveEmployee(employee);
                }

                if (saveID == null)
                {
                    response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Error;
                    response.Error = "Couldn't save employee";
                    return Json(response);
                }

                response.Data = saveID.GetValueOrDefault();
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Complete;
            }

            return Json(response);
        }
    }
}
