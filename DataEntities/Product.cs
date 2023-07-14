using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntities
{
    public class Product : BaseEntity
    {
        public int? ProductID
        {
            get; set;
        }

        public string ProductName
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public string ShortDescription
        {
            get; set;
        }

        public string ProductURL
        {
            get; set;
        }

        public string ProductMainPicture
        {
            get; set;
        }

        public List<ProductPricing> productPricings = new List<ProductPricing>();

        public List<ProductPicture> productPictures = new List<ProductPicture>();

        public List<ProductChangelog> productChangelogs = new List<ProductChangelog>();

        public class ProductPricing
        {
            public int? ProductPricingID
            {
                get; set;
            }

            public int ProductID
            {
                get; set;
            }

            public string Name
            {
                get; set;
            }

            public bool isMonthly
            {
                get; set;
            }

            public double? Amount
            {
                get; set;
            }

            public string SubText
            {
                get; set;
            }

            public bool isPrimary
            {
                get; set;
            }
        }

        public class ProductPicture
        {
            public int? ProductPictureID
            {
                get; set;
            }

            public int ProductID
            {
                get; set;
            }

            public string Path
            {
                get; set;
            }

            public string Alt
            {
                get; set;
            }
        }

        public class ProductChangelog
        {
            public int? ProductChangeLogID
            {
                get; set;
            }

            public int ProductID
            {
                get; set;
            }

            public string Version
            {
                get; set;
            }

            public string HTML
            {
                get; set;
            }
        }
    }
}
