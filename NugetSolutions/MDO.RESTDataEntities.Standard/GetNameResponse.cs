using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class GetNameResponse
    {
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
                {
                    return Username;
                }
                else
                {
                    if (string.IsNullOrEmpty(MiddleName))
                    {
                        return FirstName + " " + LastName;
                    }
                    else
                    {
                        return FirstName + " " + MiddleName.ToUpper().Substring(0, 1) + " " + LastName;
                    }
                }
            }
        }

        public string Username
        {
            get; set;
        }
        public string FirstName
        {
            get; set;
        }

        public string MiddleName
        {
            get; set;
        }

        public string LastName
        {
            get; set;
        }
    }
}
