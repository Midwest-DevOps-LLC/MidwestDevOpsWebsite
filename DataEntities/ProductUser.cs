using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntities
{
    public class ProductUser : BaseEntity
    {
        public int? ProductUserID
        {
            get; set;
        }

        public int ProductID
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

        public int UserID
        {
            get; set;
        }

        public string Key
        {
            get; set;
        }
    }
}
