using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class EmailRegistration
    {
        public int? EmailRegistrationID
        {
            get; set;
        }

        public string UUID
        {
            get; set;
        }

        public int UserID
        {
            get; set;
        }

        public bool Active
        {
            get; set;
        }

        public string Application
        {
            get; set;
        }

        public enum RegistrationStatus
        {
            Success = 1,
            Error = 2,
            ReSentEmail = 3
        }
    }
}
