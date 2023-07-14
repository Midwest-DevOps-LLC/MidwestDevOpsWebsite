using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MidwestDevOpsWebsite.Controllers
{
    public class ProductController : BaseController
    {
        public ProductController(IConfiguration configuration) : base (configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var products = new List<Models.ProductModel>();

            using (var productBLL = new BusinessLogicLayer.Products(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var p = productBLL.GetAllProducts();

                if (p != null)
                {
                    foreach (var product in p)
                    {
                        products.Add(new Models.ProductModel(product));
                    }
                }
            }

            return View(products);
        }

        public IActionResult User(int id)
        {
            if (UserSession == null)
            {
                return RedirectToAction("Home", "Login");
            }

            if (UserSession.UserID == id)
            {
                using (var userBll = new BusinessLogicLayer.Users(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                {
                    var user = userBll.GetUserByID(id);

                    if (user != null && user != new DataEntities.User())
                    {
                        var products = new List<Models.ProductUserModel>();

                        using (var productUserBLL = new BusinessLogicLayer.ProductUsers(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                        {
                            var p = productUserBLL.GetProductUserByUserID(user.UserID.Value);

                            if (p != null)
                            {
                                foreach (var product in p)
                                {
                                    products.Add(new Models.ProductUserModel(product));
                                }
                            }
                        }

                        return View(products);
                    }
                }

                return NotFound();
            }

            return Unauthorized();
        }

        public IActionResult View (int id)
        {
            var products = new Models.ProductModel();

            using (var productBLL = new BusinessLogicLayer.Products(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var p = productBLL.GetProductByID(id);

                if (p != null)
                {
                    products = new Models.ProductModel(p);
                }
                else
                {
                    return NotFound();
                }
            }

            return View(products);
        }
    }
}