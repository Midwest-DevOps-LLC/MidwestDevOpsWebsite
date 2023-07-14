using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDO.Utility.Standard;
using System.Linq;

namespace RESTBLL
{
    public class Employees : BLLManager, IDisposable
    {
        public Employees(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Employees(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<MDO.RESTDataEntities.Standard.Employee> GetAllEmployees(bool? active)
        {
            try
            {
                RESTDLL.Employees userDLL = new RESTDLL.Employees(GetConnection());

                return userDLL.GetAllEmployees(active);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.Employee GetEmployeeByID(int employeeID)
        {
            try
            {
                RESTDLL.Employees userDLL = new RESTDLL.Employees(GetConnection());

                return userDLL.GetEmployeeByID(employeeID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public MDO.RESTDataEntities.Standard.Employee GetEmployeeByUserID(int userID)
        {
            try
            {
                RESTDLL.Employees userDLL = new RESTDLL.Employees(GetConnection());

                return userDLL.GetEmployeeByUserID(userID);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }

        public long? SaveEmployee(MDO.RESTDataEntities.Standard.Employee employee)
        {
            try
            {
                RESTDLL.Employees userDLL = new RESTDLL.Employees(GetConnection());

                //var employee = userDLL.GetEmployeeByUserID(userSession.UserID);

                //if (employee != null)
                //{
                //    return employee.EmployeeID;
                //}

                return userDLL.SaveEmployee(employee);
            }
            catch (Exception e)
            {
                MDO.Utility.Standard.LogHandler.SaveException(e);
            }

            return null;
        }
    }
}
