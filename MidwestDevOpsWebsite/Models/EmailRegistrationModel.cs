using System;
using System.Collections.Generic;
using System.Text;

namespace MidwestDevOpsWebsite.Models
{
    public class EmailRegistrationModel
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

        public EmailRegistrationModel()
        {

        }

        public EmailRegistrationModel(DataEntities.EmailRegistration p)
        {
            this.EmailRegistrationID = p.EmailRegistrationID;
            this.UserID = p.UserID;
            this.UUID = p.UUID;
            this.Active = p.Active;
        }

        public DataEntities.EmailRegistration ConvertToEntity()
        {
            DataEntities.EmailRegistration p = new DataEntities.EmailRegistration();

            p.EmailRegistrationID = this.EmailRegistrationID;
            p.UserID = this.UserID;
            p.UUID = this.UUID;
            p.Active = this.Active;

            return p;
        }
    }
}
