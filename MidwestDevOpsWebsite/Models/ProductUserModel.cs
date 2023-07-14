using System;
using System.Collections.Generic;
using System.Text;

namespace MidwestDevOpsWebsite.Models
{
    public class ProductUserModel
    {
        public int? ProductUserID
        {
            get; set;
        }

        public int ProductID
        {
            get; set;
        }

        public int UserID
        {
            get; set;
        }

        public string Key
        {
            get; set;
        }

        public string ProductName
        {
            get; set;
        }

        public string ProductDescription
        {
            get; set;
        }

        public string ProductUrl
        {
            get; set;
        }

        public string ProductPicture
        {
            get; set;
        }

        public bool Active
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

        public ProductUserModel()
        {

        }

        public ProductUserModel(DataEntities.ProductUser p)
        {
            this.ProductID = p.ProductID;
            this.ProductUserID = p.ProductUserID;
            this.ProductName = p.ProductName;
            this.ProductDescription = p.ProductDescription;
            this.ProductUrl = p.ProductUrl;
            this.ProductPicture = p.ProductPicture;
            this.UserID = p.UserID;
            this.Key = p.Key;
            this.Active = p.Active;
            this.CreatedBy = p.CreatedBy;
            this.CreatedDate = p.CreatedDate;
            this.ModifiedBy = p.ModifiedBy;
            this.ModifiedDate = p.ModifiedDate;
        }

        public DataEntities.ProductUser ConvertToEntity()
        {
            DataEntities.ProductUser p = new DataEntities.ProductUser();

            p.ProductID = this.ProductID;
            p.ProductUserID = this.ProductUserID;
            p.ProductName = this.ProductName;
            p.ProductDescription = this.ProductDescription;
            p.ProductUrl = this.ProductUrl;
            p.ProductPicture = this.ProductPicture;
            p.UserID = this.UserID;
            p.Key = this.Key;
            p.Active = this.Active;
            p.CreatedBy = this.CreatedBy;
            p.CreatedDate = this.CreatedDate;
            p.ModifiedBy = this.ModifiedBy;
            p.ModifiedDate = this.ModifiedDate;

            return p;
        }
    }
}
