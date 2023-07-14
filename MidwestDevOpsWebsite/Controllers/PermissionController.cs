using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MidwestDevOpsWebsite.Controllers
{
    public class PermissionController : BaseController
    {
        public PermissionController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult View(int? id)
        {
            MDO.RESTServiceRequestor.Standard.PermissionRequestor permRequest = new MDO.RESTServiceRequestor.Standard.PermissionRequestor(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));

            var allPerms = permRequest.GetAllPermissions();

            return View(allPerms.Data);
        }        
        
        [HttpPost]
        public IActionResult SaveUserPerms([FromBody]r re)
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


            MDO.RESTServiceRequestor.Standard.UserPermissionRequestor permRequest = new MDO.RESTServiceRequestor.Standard.UserPermissionRequestor(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));

            List<MDO.RESTDataEntities.Standard.UserPermission> userPerms = new List<MDO.RESTDataEntities.Standard.UserPermission>();

            foreach (var perm in re.data)
            {
                var userPerm = new MDO.RESTDataEntities.Standard.UserPermission();

                userPerm.PermissionID = perm.id;
                userPerm.UserID = re.userID;
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

        public class r
        {
            public int userID { get; set; }
            public List<rr> data { get; set; }

            public class rr
            {
                public int id { get; set; }
                public bool val { get; set; }
            }
        }

        [HttpGet]
        public IActionResult GetPerms()
        {
            MDO.RESTServiceRequestor.Standard.PermissionRequestor permRequest = new MDO.RESTServiceRequestor.Standard.PermissionRequestor(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));

            var allPerms = permRequest.GetAllDBPermissions();


            var ret = new List<Node>();

            foreach (var perm in allPerms.Data)
            {
                if (perm.ParentPermissionID == null)
                {
                    var node = new Node();
                    node.text = perm.Name;
                    node.isChecked = false;
                    node.nodes = GetChildren(allPerms.Data, perm.ID);

                    ret.Add(node);
                }
            }

            //var nodes = GetChildren(allPerms);


            return Json(ret);
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

        [HttpGet]
        public IActionResult GetPermsForUser(int userID)
        {
            MDO.RESTServiceRequestor.Standard.PermissionRequestor permRequest = new MDO.RESTServiceRequestor.Standard.PermissionRequestor(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));
            MDO.RESTServiceRequestor.Standard.UserPermissionRequestor userPermRequest = new MDO.RESTServiceRequestor.Standard.UserPermissionRequestor(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));


            var allPerms = permRequest.GetAllDBPermissions();

            var allUserPerms = userPermRequest.GetAllUserPermissionsByUserID(userID);

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
                    if (perm.UserAlwaysHas)
                    {
                        perm.isChecked = true;
                    }
                    else
                    {
                        perm.isChecked = userPermCheck.Active;
                    }
                }
                else
                {
                    perm.isChecked = false;
                }
            }

            //foreach (var userPerm in allUserPerms.Data)
            //{
            //    var allPermCheck = allPermsBind.Where(x => x.ID == userPerm.PermissionID).FirstOrDefault();

            //    allPermCheck.isChecked = userPerm.Active;
            //    //if (allPermCheck != null)
            //    //{
            //    //    if (allPermCheck.UserAlwaysHas)
            //    //    {
            //    //        allPermCheck.isChecked = true;
            //    //    }
            //    //    else
            //    //    {
            //    //        allPermCheck.isChecked = userPerm.Active;
            //    //    }
            //    //}
            //}


            var ret = new List<Node>();

            foreach (var perm in allPermsBind)
            {
                if (perm.ParentPermissionID == null)
                {
                    var node = new Node();
                    node.text = perm.Name;
                    node.isChecked = perm.isChecked;
                    node.canEdit = !perm.UserAlwaysHas;
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
                        canEdit = !c.UserAlwaysHas,
                        nodes = GetChildren(comments, c.ID.GetValueOrDefault())
                    })
                    .ToList();

            if (r.Count <= 0)
                r = null;

            return r;
        }

        public List<Node> GetChildren(List<MDO.RESTDataEntities.Standard.Permission> comments, int? parentId)
        {
            var r = comments
                    .Where(c => c.ParentPermissionID == parentId).OrderBy(x => x.Ordinal)
                    .Select(c => new Node
                    {
                        text = c.Name,
                        //isChecked = c.isChecked,
                        nodes = GetChildren(comments, c.ID.GetValueOrDefault())
                    })
                    .ToList();

            if (r.Count <= 0)
                r = null;

            return r;
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

        public class SavePermResponse
        {
            
        }
    }
}
