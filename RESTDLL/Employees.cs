using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RESTDLL
{
    public class Employees : DBManager
    {

        #region BoringStuff

        string ConnectionString { get; set; }

        MySqlConnection sqlConnection { get; set; }

        public Employees(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Employees(MySqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        internal MySqlConnection GetConnection()
        {
            if (sqlConnection == null)
            {
                return new MySqlConnection(this.ConnectionString);
            }
            else
            {
                return this.sqlConnection;
            }
        }

        #endregion

        string baseSQL = "SELECT * from employee ";

        public List<MDO.RESTDataEntities.Standard.Employee> GetAllEmployees(bool? active)
        {
            List<MDO.RESTDataEntities.Standard.Employee> p = new List<MDO.RESTDataEntities.Standard.Employee>();

            var s = GetConnection();

            string sql = baseSQL;

            if (active.HasValue)
            {
                if (active.Value == true)
                    sql += " where Active = 1;";
                else
                    sql += " where Active = 0;";
            }

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        p.Add(ConvertMySQLToEntity(reader));
                    }
                }
            }

            return p;
        }

        public MDO.RESTDataEntities.Standard.Employee GetEmployeeByID(int siteID)
        {
            MDO.RESTDataEntities.Standard.Employee person = new MDO.RESTDataEntities.Standard.Employee();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(baseSQL + " Where employeeID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", siteID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return ConvertMySQLToEntity(reader);
                    }

                    return null;
                }
            }
        }

        public MDO.RESTDataEntities.Standard.Employee GetEmployeeByUserID(int userID)
        {
            MDO.RESTDataEntities.Standard.Employee person = new MDO.RESTDataEntities.Standard.Employee();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(baseSQL + " Where UserID = @ID;", conn);

                cmd.Parameters.AddWithValue("@ID", userID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return ConvertMySQLToEntity(reader);
                    }

                    return null;
                }
            }
        }


        public long? SaveEmployee(MDO.RESTDataEntities.Standard.Employee p)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                string sql = "";

                if (p.EmployeeID == null)
                {
                    sql = @"INSERT INTO `employee` (`EmployeeID`, `UserID`, `Title`, Admin, Active, HireDate) VALUES (NULL, @UserID, @Title, @Admin, @Active, @HireDate);
                            SELECT LAST_INSERT_ID();";
                }
                else
                {
                    sql = @"UPDATE `employee` SET UserID = @UserID, Title = @Title, Admin = @Admin, Active = @Active, HireDate = @HireDate WHERE EmployeeID = @EmployeeID;";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@EmployeeID", p.EmployeeID);
                cmd.Parameters.AddWithValue("@UserID", p.UserID);;
                cmd.Parameters.AddWithValue("@Title", p.Title);
                cmd.Parameters.AddWithValue("@Admin", p.IsAdmin);
                cmd.Parameters.AddWithValue("@Active", p.Active);
                cmd.Parameters.AddWithValue("@HireDate", p.HireDate);

                if (p.EmployeeID == null) //Get new id number
                {
                    return cmd.ExecuteScalar().ToString().ConvertToNullableInt();
                }
                else //Return id if worked
                {
                    cmd.ExecuteScalar();

                    return p.EmployeeID;
                }
            }
        }

        internal MDO.RESTDataEntities.Standard.Employee ConvertMySQLToEntity(MySqlDataReader reader)
        {
            MDO.RESTDataEntities.Standard.Employee p = new MDO.RESTDataEntities.Standard.Employee();

            p.EmployeeID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "EmployeeID"));
            p.UserID = Convert.ToInt32(DBUtilities.ReturnSafeInt(reader, "UserID"));
            p.Title = DBUtilities.ReturnSafeString(reader, "Title");
            p.IsAdmin = DBUtilities.ReturnBoolean(reader, "Admin");
            p.Active = DBUtilities.ReturnBoolean(reader, "Active");
            p.HireDate = DBUtilities.ReturnDateTime(reader, "HireDate");

            return p;
        }
    }
}
