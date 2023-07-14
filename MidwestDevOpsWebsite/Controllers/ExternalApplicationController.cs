using MDO.RESTServiceRequestor.Standard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MidwestDevOpsWebsite.Controllers
{
    public class ExternalApplicationController : BaseController
    {
        public ExternalApplicationController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            MDO.RESTServiceRequestor.Standard.ExternalApplicationRequest permRequest = new MDO.RESTServiceRequestor.Standard.ExternalApplicationRequest(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));

            var externalApplication = permRequest.GetAll().Data;


            return View(externalApplication);
        }

        public IActionResult View(int? id)
        {
            MDO.RESTServiceRequestor.Standard.ExternalApplicationRequest permRequest = new MDO.RESTServiceRequestor.Standard.ExternalApplicationRequest(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));

            var externalApplication = new MDO.RESTDataEntities.Standard.ExternalApplication();

            if (id.HasValue)
            {
                externalApplication = permRequest.Get(id.GetValueOrDefault()).Data;
            }

            return View(externalApplication);
        }

        [HttpGet]
        public IActionResult GetPerms()
        {
            MDO.RESTServiceRequestor.Standard.PermissionRequestor permRequest = new MDO.RESTServiceRequestor.Standard.PermissionRequestor(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));

            var allPerms = permRequest.GetAllDBPermissions();


            var ret = new List<int>();

            return Json(ret);
        }

        [HttpPost]
        public IActionResult Update(Models.ExternalApplicationModel model)
        {
            var loginRequestor = new ExternalApplicationRequest(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));

            var request = new MDO.RESTDataEntities.Standard.ExternalApplication();

            request.ID = model.ID;
            request.Name = model.Name;
            request.Description = model.Description;

            var response = loginRequestor.Put(request);

            return Json(response.ValidationModel);
        }

        public class r
        {
            public int externalApplicationID { get; set; }
            public List<rr> data { get; set; }

            public class rr
            {
                public int id { get; set; }
                public bool val { get; set; }
            }
        }

        [HttpPost]
        public IActionResult SavePerms([FromBody] r re)
        {
            if (UserSession == null)
            {
                return Unauthorized();
            }

            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>();

            //var canEditUserPermission = HasPermission(MDO.RESTDataEntities.Standard.Permission.PermissionLookup.UserPermission.CreateUpdate);

            //if (canEditUserPermission == false)
            //{
            //    response.AddError("You don't have permission to change a user's permission");
            //    response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Error;
            //    response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;

            //    return Json(response.ValidationModel);
            //}


            MDO.RESTServiceRequestor.Standard.ExternalApplicationPermissionRequestor permRequest = new MDO.RESTServiceRequestor.Standard.ExternalApplicationPermissionRequestor(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));

            List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission> userPerms = new List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>();

            foreach (var perm in re.data)
            {
                var userPerm = new MDO.RESTDataEntities.Standard.ExternalApplication.Permission();

                userPerm.PermissionID = perm.id;
                userPerm.ExternalApplicationID = re.externalApplicationID;
                userPerm.Active = perm.val;

                userPerms.Add(userPerm);
            }



            response = permRequest.PutMultiple(userPerms);

            //foreach (var perm in userPerms)
            //{
            //    response = permRequest.Put(perm);

            //    if (response.Status == MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Error)
            //        break;
            //}

            return Json(response.ValidationModel);
        }

        [HttpGet]
        public IActionResult GetPermsForExternalApplication(int externalApplicationID)
        {
            MDO.RESTServiceRequestor.Standard.PermissionRequestor permRequest = new MDO.RESTServiceRequestor.Standard.PermissionRequestor(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));
            MDO.RESTServiceRequestor.Standard.ExternalApplicationPermissionRequestor userPermRequest = new MDO.RESTServiceRequestor.Standard.ExternalApplicationPermissionRequestor(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));


            var allPerms = permRequest.GetAllDBPermissions();

            if (allPerms.Status == MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>>.StatusEnum.Error)
            {
                return Json(allPerms);
            }

            var allUserPerms = userPermRequest.GetAllExternalApplicationPermissionsByExternalApplicationID(externalApplicationID);

            if (allUserPerms.Status == MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.ExternalApplication.Permission>>.StatusEnum.Error)
            {
                return Json(allUserPerms);
            }

            var allPermsBind = new List<UserPermissionBinder>();

            foreach (var allPerm in allPerms.Data.OrderBy(x => x.Ordinal))
            {
                var r = new UserPermissionBinder();

                allPermsBind.Add(r.Convert(allPerm));
            }

            foreach (var perm in allPermsBind.OrderBy(x => x.Ordinal))
            {
                var userPermCheck = allUserPerms.Data.Where(x => x.PermissionID == perm.ID).FirstOrDefault();

                if (userPermCheck != null)
                {
                    perm.isChecked = userPermCheck.Active;
                }
                else
                {
                    perm.isChecked = false;
                }
            }

            var ret = new List<Node>();

            foreach (var perm in allPermsBind)
            {
                if (perm.ParentPermissionID == null)
                {
                    var node = new Node();
                    node.text = perm.Name;
                    node.isChecked = perm.isChecked;
                    node.canEdit = true;
                    node.permissionID = perm.ID.GetValueOrDefault();
                    node.nodes = GetChildren(allPermsBind, perm.ID);

                    ret.Add(node);
                }
            }

            //var nodes = GetChildren(allPerms);


            return Json(ret);
        }

        public List<Node> GetChildren(List<UserPermissionBinder> comments, int? parentId)
        {
            var r = comments
                    .Where(c => c.ParentPermissionID == parentId).OrderBy(x => x.Ordinal)
                    .Select(c => new Node
                    {
                        text = c.Name,
                        isChecked = c.isChecked,
                        permissionID = c.ID.GetValueOrDefault(),
                        canEdit = true,
                        nodes = GetChildren(comments, c.ID.GetValueOrDefault())
                    })
                    .ToList();

            if (r.Count <= 0)
                r = null;

            return r;
        }

        public class UserPermissionBinder : MDO.RESTDataEntities.Standard.Permission
        {
            public bool isChecked { get; set; }
            //public int permissionID { get; set; }

            public UserPermissionBinder Convert(MDO.RESTDataEntities.Standard.Permission perm)
            {
                var ret = new UserPermissionBinder();

                ret.CreatedDate = perm.CreatedDate;
                ret.Description = perm.Description;
                ret.ID = perm.ID;
                ret.Name = perm.Name;
                ret.Ordinal = perm.Ordinal;
                ret.ParentPermissionID = perm.ParentPermissionID;
                ret.Children = perm.Children;
                ret.UserAlwaysHas = perm.UserAlwaysHas;
                //ret.permissionID = perm.ID.GetValueOrDefault();

                return ret;
            }
        }

        public class Node
        {
            public string text { get; set; }
            public string icon { get; set; }
            public string href { get; set; }
            [Description("class")]
            public string class_prop { get; set; }
            public string id { get; set; }
            public bool isChecked { get; set; }
            public int permissionID { get; set; }
            public bool canEdit { get; set; }

            public List<Node> nodes { get; set; } = null;
        }
    }
}
