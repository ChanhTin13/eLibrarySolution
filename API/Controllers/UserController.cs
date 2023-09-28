using BaseAPI.Controllers;
using Entities;
using Entities.Search;
using Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CoreContants;

namespace API.Controllers
{
    [Route("api/User")]
    [ApiController]
    [Description("Người dùng")]
    [Authorize]
    public class UserController : BaseController<tbl_Users, UserCreate, UserUpdate, UserSearch>
    {
        protected IUserService userService;
        private IConfiguration configuration;
        private IRoleService roleService;
        public UserController(IServiceProvider serviceProvider, ILogger<BaseController<tbl_Users, UserCreate, UserUpdate, UserSearch>> logger
            , IConfiguration configuration
            , IRoleService roleService
            , IWebHostEnvironment env) : base(serviceProvider, logger, env)
        {
            this.domainService = serviceProvider.GetRequiredService<IUserService>();
            this.userService = serviceProvider.GetRequiredService<IUserService>();
            this.configuration = configuration;
            this.roleService = roleService;
        }
        /// <summary>
        /// Thêm mới item dữ liệu
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới")]
        public override async Task<AppDomainResult> AddItem([FromBody] UserCreate itemModel)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            bool success = false;
            if (ModelState.IsValid)
            {
                var item = mapper.Map<tbl_Users>(itemModel);
                if (item != null)
                {
                    await Validate(item);
                    List<RoleInUser> roleInUsers = new List<RoleInUser>();
                    var roleCodes = itemModel.roleCodes.Split(',');
                    if (!roleCodes.Any())
                        throw new AppException("Không tìm thấy quyền");
                    var roleConfigs = new List<RoleInUser>()
                    { 
                        new RoleInUser { code = role.admin.ToString(), name = "Quản trị viên"},
                        new RoleInUser { code = role.moderator.ToString(), name = "Điều phối viên"},
                        new RoleInUser { code = role.teacher.ToString(), name = "Giảng viên"},
                        new RoleInUser { code = role.student.ToString(), name = "Học viên"}
                    };
                    foreach (var roleCode in roleCodes)
                    {
                        var roleConfig = roleConfigs.FirstOrDefault(x => x.code == roleCode);
                        if(roleConfig == null)
                            throw new AppException("Quyền không phù hợp");
                        roleInUsers.Add(new RoleInUser { code = roleConfig.code, name = roleConfig.name });
                    }
                    item.roles = Newtonsoft.Json.JsonConvert.SerializeObject(roleInUsers);
                    item.password = SecurityUtilities.HashSHA1(item.password);
                    success = await this.domainService.CreateAsync(item);
                    if (success)
                        appDomainResult.resultCode = (int)HttpStatusCode.OK;
                    else
                        throw new Exception("Lỗi trong quá trình xử lý");
                    appDomainResult.success = success;
                }
                else
                    throw new AppException("Item không tồn tại");
            }
            else
            {
                throw new AppException(ModelState.GetErrorMessage());
            }
            return appDomainResult;
        }
        /// <summary>
        /// Lấy danh sách item phân trang
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Lấy danh sách nhân viên")]
        public override async Task<AppDomainResult> Get([FromQuery] UserSearch baseSearch)
        {
            if (ModelState.IsValid)
            {
                var baseSearchPrivate = mapper.Map<StaffSearchPrivate>(baseSearch);
                PagedList<tbl_Users> pagedData = await this.domainService.GetPagedListData(baseSearchPrivate);
                return new AppDomainResult
                {
                    data = pagedData,
                    success = true,
                    resultCode = (int)HttpStatusCode.OK
                };
            }
            else
                throw new AppException(ModelState.GetErrorMessage());
        }
        [HttpPut("edit-role")]
        [AppAuthorize]
        [Description("Cấp nhật quyền nhân viên")]
        public async Task<AppDomainResult> EditRole([FromBody] EditRoleUser model)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            bool success = false;
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var item = await this.domainService.GetByIdAsync(model.userId);
            if (item.isAdmin == true)
                throw new AppException("Không thể chỉnh sửa quyền quản trị viên");
            if (item != null)
            {
                List<RoleInUser> roleInUsers = new List<RoleInUser>();
                var roleCodes = model.roleCodes.Split(',');
                if (!roleCodes.Any())
                    throw new AppException("Không tìm thấy quyền");
                var roleConfigs = new List<RoleInUser>()
                    {
                        new RoleInUser { code = role.admin.ToString(), name = "Quản trị viên"},
                        new RoleInUser { code = role.moderator.ToString(), name = "Điều phối viên"},
                        new RoleInUser { code = role.teacher.ToString(), name = "Giảng viên"},
                        new RoleInUser { code = role.student.ToString(), name = "Học viên"},
                    };
                foreach (var roleCode in roleCodes)
                {
                    var roleConfig = roleConfigs.FirstOrDefault(x => x.code == roleCode);
                    if (roleConfig == null)
                        throw new AppException("Quyền không phù hợp");
                    roleInUsers.Add(new RoleInUser { code = roleConfig.code, name = roleConfig.name });
                }
                item.roles = Newtonsoft.Json.JsonConvert.SerializeObject(roleInUsers);
                success = await this.domainService.UpdateAsync(item);
                if (success)
                    appDomainResult.resultCode = (int)HttpStatusCode.OK;
                else
                    throw new Exception("Lỗi trong quá trình xử lý");
                appDomainResult.success = success;
            }
            else
                throw new AppException("Item không tồn tại");
            return appDomainResult;
        }
        private class RoleInUser
        { 
            public string code { get; set; }
            public string name { get; set; }
        }
    }
}
