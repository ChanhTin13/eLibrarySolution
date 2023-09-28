using Entities.Search;
using Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Request.RequestUpdate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CoreContants;

namespace API.Controllers
{
    [Route("api/Role")]
    [ApiController]
    [Description("Quản lý quyền")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        protected IRoleService roleService;
        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }
        /// <summary>
        /// Danh sách quyền
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<AppDomainResult> Get()
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            var data = await roleService.GetAsync(x => x.deleted == false && x.code != "admin");

            appDomainResult = new AppDomainResult
            {
                data = from i in data
                       select new 
                       {
                          id = i.id,
                          code = i.code,
                          name = i.name
                       },
                success = true,
                resultCode = (int)HttpStatusCode.OK
            };
            return appDomainResult;
        }
        /// <summary>
        /// Cập nhật quyền
        /// </summary>
        /// <remarks>
        /// --------------------------------- <br></br>
        /// role <br></br>
        /// admin = 1 <br></br>
        /// moderator = 2 <br></br>
        /// teacher = 3 <br></br>
        /// student = 4 <br></br>
        /// </remarks>
        /// <param name="roleUpdate"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<AppDomainResult> UpdateItem([FromBody] RoleUpdate roleUpdate)
        {
            if (!LoginContext.Instance.CurrentUser.isAdmin)
                throw new UnauthorizedAccessException("Không có quyền thực hiện");
            AppDomainResult appDomainResult = new AppDomainResult();
            bool success = false;
            if (ModelState.IsValid)
            {
                var item = await roleService.GetSingleAsync(x => x.code.ToUpper() == roleUpdate.code.ToString().ToUpper());
                if (item == null)
                    throw new AppException("Không tìm thấy quyền này");

                var permissions = JsonConvert.DeserializeObject<List<Permission>>(item.permissions);
                if (string.IsNullOrEmpty(item.permissions))
                {
                    permissions = new List<Permission>();
                    var model = new Permission
                    {
                        controller = roleUpdate.controller,
                        action = roleUpdate.action,
                        grant = true
                    };
                    permissions.Add(model);
                    item.permissions = JsonConvert.SerializeObject(permissions);
                    success = await roleService.UpdateAsync(item);
                }
                else
                {
                    var permission = permissions
                        .Where(x => x.controller.ToUpper() == roleUpdate.controller.ToUpper() && x.action.ToUpper() == roleUpdate.action.ToUpper())
                        .FirstOrDefault();
                    if (permission == null)
                    {
                        var model = new Permission
                        {
                            controller = roleUpdate.controller,
                            action = roleUpdate.action,
                            grant = true
                        };
                        permissions.Add(model);
                        item.permissions = JsonConvert.SerializeObject(permissions);
                        success = await roleService.UpdateAsync(item);
                    }
                    else
                    {
                        permissions.Remove(permission);
                        var model = new Permission
                        {
                            controller = roleUpdate.controller,
                            action = roleUpdate.action,
                            grant = !permission.grant
                        };
                        permissions.Add(model);
                        item.permissions = JsonConvert.SerializeObject(permissions);
                        success = await roleService.UpdateAsync(item);
                    }

                }
                if (success)
                    appDomainResult.resultCode = (int)HttpStatusCode.OK;
                else
                    throw new Exception("Lỗi trong quá trình xử lý");
                appDomainResult.success = success;
            }
            else
                throw new AppException(ModelState.GetErrorMessage());
            return appDomainResult;
        }
        /// <summary>
        /// Lấy danh sach phân quyền
        /// </summary>
        /// <remarks>
        /// --------------------------------- <br></br>
        /// role <br></br>
        /// admin = 1 <br></br>
        /// moderator = 2 <br></br>
        /// teacher = 3 <br></br>
        /// student = 4 <br></br>
        /// </remarks>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("permission/{code}")]
        public async Task<AppDomainResult> Get(role code)
        {
            System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
            Assembly[] assems = currentDomain.GetAssemblies();
            var controllers = new List<ControllerModel>();
            foreach (Assembly assem in assems)
            {
                var controller = assem.GetTypes().Where(type =>
                typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
                  .Select(e => new ControllerModel()
                  {
                      id = e.Name.Replace("Controller", string.Empty),
                      name = string.Format("{0}", ReflectionUtilities.GetClassDescription(e)).Replace("Controller", string.Empty),
                      actions = (from i in e.GetMembers().Where(x => (
                                 x.Module.Name == "API.dll" || 
                                 x.Module.Name == "BaseAPI.dll") 
                                 && x.Name != ".ctor" 
                                 && x.Name != "Validate" 
                                 && x.Name != "UploadFile"
                                 && x.GetCustomAttributes(typeof(NonActionAttribute)).Select(x => x.GetType().Name).FirstOrDefault() != typeof(NonActionAttribute).Name
                                 )
                                 select new ActionModel
                                 {
                                     id =$"{e.Name.Replace("Controller", string.Empty)}-{i.Name}",
                                     name = i.GetCustomAttributes(typeof(DescriptionAttribute), true)
                                                 .Cast<DescriptionAttribute>().Select(d => d.Description)
                                                 .SingleOrDefault() ?? i.Name
                                 }).OrderBy(d => d.name).ToList()
                  })
                  .Where(e => e.id != "Role" && e.id != "Auth")
                  .OrderBy(e => e.name)
                  .Distinct();

                controllers.AddRange(controller);
            }

            var role = await roleService.GetSingleAsync(x=>x.code.ToUpper() == code.ToString().ToUpper() && x.deleted == false);
            if (role == null)
                throw new AppException("Không tồn tại quyền này");
            var permissions = new List<Permission>();
            if(!string.IsNullOrEmpty(role.permissions))
                permissions = JsonConvert.DeserializeObject<List<Permission>>(role.permissions);
            var data = new RoleModel
            {
                id = role.id,
                code = role.code,
                name = role.name,
                controllers = (from i in controllers
                              select new ControllerModel
                              { 
                                 id = i.id,
                                 name = i.name,
                                 grant = code == CoreContants.role.admin ? true : permissions.Where(x=>x.controller.ToUpper() == i.id.ToUpper() && x.grant == true).Any()
                                 ? true : false,
                                 actions = (from a in i.actions
                                            select new ActionModel
                                            {
                                                id = a.id,
                                                name = a.name,
                                                grant = code == CoreContants.role.admin ? true : permissions
                                                .Where(x=>x.controller.ToUpper() == i.id.ToUpper() && x.action.ToUpper() == a.id.ToUpper() && x.grant == true).Any()
                                                ? true : false

                                            }).ToList()
                              }).ToList()
            };
            var appDomainResult = new AppDomainResult
            {
                data = data,
                success = true,
                resultCode = (int)HttpStatusCode.OK
            };
            return appDomainResult;
        }
        private class RoleModel
        {
            public Guid id { get; set; }
            public string name { get; set; }
            public string code { get; set; }
            public List<ControllerModel> controllers { get; set; }
        }
        private class ControllerModel
        {
            public string id { get; set; }

            public string name { get; set; }
            public bool grant { get; set; }
            public List<ActionModel> actions { get; set; }
        }
        private class ActionModel
        {
            public string id { get; set; }

            public string name { get; set; }
            public bool grant { get; set; }
        }
        /// <summary>
        /// Lấy danh sách menu
        /// </summary>
        /// <remarks>
        /// --------------------------------- <br></br>
        /// role <br></br>
        /// admin = 1 <br></br>
        /// moderator = 2 <br></br>
        /// teacher = 3 <br></br>
        /// student = 4 <br></br></remarks>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("permission/{code}/controller")]
        public async Task<AppDomainResult> GetController(role code)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            if (code == CoreContants.role.admin)
            {
                System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
                Assembly[] assems = currentDomain.GetAssemblies();
                var controllers = new List<string>();
                foreach (Assembly assem in assems)
                {
                    var controller = assem.GetTypes().Where(type =>
                    typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
                      .Select(e => new ControllerModel()
                      {
                          id = e.Name.Replace("Controller", string.Empty),
                      })
                      .Where(e => e.id != "Role" && e.id != "Auth")
                      .OrderBy(e => e.name)
                      .Distinct();
                    controllers.AddRange(controller.Select(x=>x.id));
                }
                return new AppDomainResult
                {
                    data = controllers,
                    success = true,
                    resultCode = (int)HttpStatusCode.OK
                };
            }
            var role = await roleService.GetSingleAsync(x => x.code == code.ToString()); 
            if (string.IsNullOrEmpty(role.permissions))
                return new AppDomainResult
                {
                    data = null,
                    success = true,
                    resultCode = (int)HttpStatusCode.OK
                };
            var permissions = JsonConvert.DeserializeObject<List<Permission>>(role.permissions);
            var data = permissions.Where(x => x.grant == true).Select(x => x.controller).Distinct().ToList();
            return new AppDomainResult
            {
                data = data,
                success = true,
                resultCode = (int)HttpStatusCode.OK
            };
        }
        /// <summary>
        /// Lấy danh sách chức năng
        /// </summary>
        /// <remarks>
        /// --------------------------------- <br></br>
        /// role <br></br>
        /// admin = 1 <br></br>
        /// moderator = 2 <br></br>
        /// teacher = 3 <br></br>
        /// student = 4 <br></br>
        /// </remarks>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet("permission/controller-action")]
        public async Task<AppDomainResult> GetAction([FromQuery] ActionSearch baseSearch)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            if (baseSearch.code == CoreContants.role.admin)
            {
                System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
                Assembly[] assems = currentDomain.GetAssemblies();
                var controllers = new List<ControllerModel>();
                foreach (Assembly assem in assems)
                {
                    var controller = assem.GetTypes().Where(type =>
                    typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
                      .Select(e => new ControllerModel()
                      {
                          id = e.Name.Replace("Controller", string.Empty),
                          actions = (from i in e.GetMembers().Where(x => (x.Module.Name == "API.dll" || x.Module.Name == "BaseAPI.dll") && x.Name != ".ctor" && x.Name != "Validate" && x.Name != "UploadFile")
                                     select new ActionModel
                                     {
                                         id = $"{e.Name.Replace("Controller", string.Empty)}-{i.Name}"
                                     }).OrderBy(d => d.name).ToList()
                      })
                      .Where(e => e.id == baseSearch.controller)
                      .OrderBy(e => e.name)
                      .Distinct();
                    controllers.AddRange(controller);
                }
                return new AppDomainResult
                {
                    data = controllers.FirstOrDefault() == null ? null 
                    : controllers.FirstOrDefault().actions.Select(x=>x.id).ToArray(),
                    success = true,
                    resultCode = (int)HttpStatusCode.OK
                };
            }
            var role = await roleService.GetSingleAsync(x => x.code == baseSearch.code.ToString());
            if(string.IsNullOrEmpty(role.permissions)) 
                return new AppDomainResult
                {
                    data = null,
                    success = true,
                    resultCode = (int)HttpStatusCode.OK
                };
            var permissions = JsonConvert.DeserializeObject<List<Permission>>(role.permissions);
            var data = permissions.Where(x => x.grant == true && x.controller == baseSearch.controller).Select(x => x.action).ToList();
            return new AppDomainResult
            {
                data = data,
                success = true,
                resultCode = (int)HttpStatusCode.OK
            };
        }
    }
}
