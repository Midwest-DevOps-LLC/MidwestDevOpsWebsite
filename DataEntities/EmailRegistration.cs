using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntities
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

        public string Application { get; set; }
    }
}
