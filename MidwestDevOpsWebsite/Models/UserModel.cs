using System;
using System.Collections.Generic;
using System.Text;

namespace MidwestDevOpsWebsite.Models
{
    public class UserModel
    {
        public int? UserID
        {
            get; set;
        }

        public string UUID
        {
            get; set;
        }

        public string Username
        {
            get; set;
        }

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(FirstName) == false)
                {
                    return FirstName;
                }else if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) == false)
                {
                    return LastName;
                }
                else if (string.IsNullOrEmpty(FirstName) == false && string.IsNullOrEmpty(LastName) == false)
                {
                    return LastName + ", " + FirstName;
                }
                else
                {
                    return "";
                }
            }
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string Email
        {
            get; set;
        }

        public string Password
        {
            get; set;
        }

        public bool Active
        {
            get; set;
        }

        public bool Activated
        {
            get; set;
        }

        public int? CreatedBy
        {
            get; set;
        }

        public DateTime? CreatedDate
        {
            get; set;
        }

        public int? ModifiedBy
        {
            get; set;
        }

        public DateTime? ModifiedDate
        {
            get; set;
        }

        public List<DataEntities.ThirdParty> ThirdParties { get; set; } = new List<DataEntities.ThirdParty>();
        public List<DataEntities.ThirdPartyUser> ThirdPartyUser { get; set; } = new List<DataEntities.ThirdPartyUser>();

        public bool CanViewPermissions { get; set; }

        public UserModel()
        {

        }

        public UserModel(DataEntities.User p)
        {
            this.UserID = p.UserID;
            this.UUID = p.UUID;
            this.Username = p.Username;
            this.FirstName = p.FirstName;
            this.MiddleName = p.MiddleName;
            this.LastName = p.LastName;
            this.Email = p.Email;
            this.Password = p.Password;
            this.Active = p.Active;
            this.Activated = p.Activated;
            this.CreatedBy = p.CreatedBy;
            this.CreatedDate = p.CreatedDate;
            this.ModifiedBy = p.ModifiedBy;
            this.ModifiedDate = p.ModifiedDate;
        }

        public DataEntities.User ConvertToEntity()
        {
            DataEntities.User p = new DataEntities.User();

            p.UserID = this.UserID;
            p.UUID = this.UUID;
            p.Username = this.Username;
            p.FirstName = this.FirstName;
            p.MiddleName = this.MiddleName;
            p.LastName = this.LastName;
            p.Email = this.Email;
            p.Password = this.Password;
            p.Active = this.Active;
            p.Activated = this.Activated;
            p.CreatedBy = this.CreatedBy;
            p.CreatedDate = this.CreatedDate;
            p.ModifiedBy = this.ModifiedBy;
            p.ModifiedDate = this.ModifiedDate;

            return p;
        }

        public class UserModelOnlineModel
        {
            public int UserID
            {
                get; set;
            }

            public string UserName
            {
                get; set;
            }

            public int UserSessionStatusLID
            {
                get; set;
            }

            public DateTime? LastOnline
            {
                get; set;
            }
        }
    }
}
