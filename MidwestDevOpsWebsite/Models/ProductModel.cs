using System;
using System.Collections.Generic;
using System.Text;

namespace MidwestDevOpsWebsite.Models
{
    public class ProductModel
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

        public ProductModel()
        {

        }

        public List<ProductPricing> productPricings = new List<ProductPricing>();

        public List<ProductPicture> productPictures = new List<ProductPicture>();

        public List<ProductChangelog> productChangelogs = new List<ProductChangelog>();

        public ProductModel(DataEntities.Product p)
        {
            this.ProductID = p.ProductID;
            this.ProductName = p.ProductName;
            this.Description = p.Description;
            this.ShortDescription = p.ShortDescription;
            this.ProductURL = p.ProductURL;
            this.ProductMainPicture = p.ProductMainPicture;
            this.Active = p.Active;
            this.CreatedBy = p.CreatedBy;
            this.CreatedDate = p.CreatedDate;
            this.ModifiedBy = p.ModifiedBy;
            this.ModifiedDate = p.ModifiedDate;

            List<ProductPricing> pp = new List<ProductPricing>();

            foreach (var ppe in p.productPricings)
            {
                pp.Add(new ProductPricing(ppe));
            }

            List<ProductPicture> ppic = new List<ProductPicture>();

            foreach (var ppe in p.productPictures)
            {
                ppic.Add(new ProductPicture(ppe));
            }

            List<ProductChangelog> pc = new List<ProductChangelog>();

            foreach (var ppe in p.productChangelogs)
            {
                pc.Add(new ProductChangelog(ppe));
            }

            this.productPricings = pp;
            this.productPictures = ppic;
            this.productChangelogs = pc;
        }

        public DataEntities.Product ConvertToEntity()
        {
            DataEntities.Product p = new DataEntities.Product();

            p.ProductID = this.ProductID;
            p.ProductName = this.ProductName;
            p.Description = this.Description;
            p.ShortDescription = this.ShortDescription;
            p.ProductURL = this.ProductURL;
            p.ProductMainPicture = this.ProductMainPicture;
            p.Active = this.Active;
            p.CreatedBy = this.CreatedBy;
            p.CreatedDate = this.CreatedDate;
            p.ModifiedBy = this.ModifiedBy;
            p.ModifiedDate = this.ModifiedDate;

            List<DataEntities.Product.ProductPricing> pp = new List<DataEntities.Product.ProductPricing>();

            foreach (var ppe in this.productPricings)
            {
                pp.Add(ppe.ConvertToEntity());
            }

            List<DataEntities.Product.ProductPicture> ppic = new List<DataEntities.Product.ProductPicture>();

            foreach (var ppe in this.productPictures)
            {
                ppic.Add(ppe.ConvertToEntity());
            }

            List<DataEntities.Product.ProductChangelog> pc = new List<DataEntities.Product.ProductChangelog>();

            foreach (var ppe in this.productChangelogs)
            {
                pc.Add(ppe.ConvertToEntity());
            }

            p.productPricings = pp;
            p.productPictures = ppic;
            p.productChangelogs = pc;

            return p;
        }

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

            public DataEntities.Product.ProductPricing ConvertToEntity()
            {
                DataEntities.Product.ProductPricing p = new DataEntities.Product.ProductPricing();

                p.ProductPricingID = this.ProductPricingID;
                p.ProductID = this.ProductID;
                p.Name = this.Name;
                p.isMonthly = this.isMonthly;
                p.Amount = this.Amount;
                p.SubText = this.SubText;
                p.isPrimary = this.isPrimary;

                return p;
            }

            public ProductPricing(DataEntities.Product.ProductPricing p)
            {
                this.ProductPricingID = p.ProductPricingID;
                this.ProductID = p.ProductID;
                this.Name = p.Name;
                this.isMonthly = p.isMonthly;
                this.Amount = p.Amount;
                this.SubText = p.SubText;
                this.isPrimary = p.isPrimary;
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

            public DataEntities.Product.ProductPicture ConvertToEntity()
            {
                DataEntities.Product.ProductPicture p = new DataEntities.Product.ProductPicture();

                p.ProductPictureID = this.ProductPictureID;
                p.ProductID = this.ProductID;
                p.Path = this.Path;
                p.Alt = this.Alt;

                return p;
            }

            public ProductPicture(DataEntities.Product.ProductPicture p)
            {
                this.ProductPictureID = p.ProductPictureID;
                this.ProductID = p.ProductID;
                this.Path = p.Path;
                this.Alt = p.Alt;
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

            public DataEntities.Product.ProductChangelog ConvertToEntity()
            {
                DataEntities.Product.ProductChangelog p = new DataEntities.Product.ProductChangelog();

                p.ProductChangeLogID = this.ProductChangeLogID;
                p.ProductID = this.ProductID;
                p.Version = this.Version;
                p.HTML = this.HTML;

                return p;
            }

            public ProductChangelog(DataEntities.Product.ProductChangelog p)
            {
                this.ProductChangeLogID = p.ProductChangeLogID;
                this.ProductID = p.ProductID;
                this.Version = p.Version;
                this.HTML = p.HTML;
            }
        }
    }
}
