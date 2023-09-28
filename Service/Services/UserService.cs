using Entities;
using Extensions;
using Interface.DbContext;
using Interface.Services;
using Interface.UnitOfWork;
using Utilities;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Service.Services.DomainServices;
using Entities.Search;
using Newtonsoft.Json;
using static Utilities.CoreContants;
using Microsoft.Extensions.Configuration;
using Request.DomainRequests;
using Request.Auth;
using Entities.DomainEntities;
using Request.RequestUpdate;
using System.Threading;

namespace Service.Services
{
    public class UserService : DomainService<tbl_Users, UserSearch>, IUserService
    {
        protected IAppDbContext coreDbContext;
        private IRoleService roleService;
        private INecessaryService necessaryService;
        private IConfiguration configuration;
        public UserService(IAppUnitOfWork unitOfWork, IMapper mapper, IAppDbContext coreDbContext, IRoleService roleService, INecessaryService necessaryService, IConfiguration configuration) : base(unitOfWork, mapper)
        {
            this.necessaryService = necessaryService;
            this.coreDbContext = coreDbContext;
            this.roleService = roleService;
            this.configuration = configuration;
        }

        protected override string GetStoreProcName()
        {
            return "Get_Users";
        }

        /// <summary>
        /// Cập nhật password mới cho user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserPassword(Guid userId, string newPassword)
        {
            bool result = false;

            var existUserInfo = await this.unitOfWork.Repository<tbl_Users>().GetQueryable().Where(e => e.id == userId).FirstOrDefaultAsync();
            if (existUserInfo != null)
            {
                existUserInfo.password = newPassword;
                existUserInfo.updated = Timestamp.Now();
                Expression<Func<tbl_Users, object>>[] includeProperties = new Expression<Func<tbl_Users, object>>[]
                {
                    e => e.password,
                    e => e.updated
                };
                await this.unitOfWork.Repository<tbl_Users>().UpdateFieldsSaveAsync(existUserInfo, includeProperties);
                await this.unitOfWork.SaveAsync();
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra user đăng nhập
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> Verify(string userName, string password)
        {

            var user = await Queryable
                .Where(e => e.deleted == false
                && (e.username == userName
                //|| e.phone == userName
                //|| e.email == userName
                ))
                .FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.active == false)
                {
                    throw new Exception("Tài khoản chưa được kích hoạt");
                }
                if (user.password == SecurityUtilities.HashSHA1(password))
                {
                    return true;
                }
                else
                    return false;

            }
            else
                return false;
        }
        public async Task<bool> HasPermission(Guid userId, string controller, string action)
        {
            if (controller == "Auth")
                return true;
            action = $"{controller}-{action}";
            var user = await GetByIdAsync(userId);
            var roles = await roleService.GetAsync(
                x => user.roles.ToUpper().Contains(x.code.ToUpper()) 
                && x.deleted == false);
            var permissions = new List<Permission>();
            if (!roles.Any())
                return false;
            foreach (var item in roles)
            {
                if (string.IsNullOrEmpty(item.permissions))
                    return false;
                permissions.AddRange(JsonConvert.DeserializeObject<List<Permission>>(item.permissions));
            }
            if (permissions
                       .Where(x => x.controller.ToUpper() == controller.ToUpper()
                       && x.action.ToUpper() == action.ToUpper()
                       && x.grant == true).Any())
                return true;
            return false;
        }
        public async Task ValidatePassword(Guid userId, string password)
        {
            password = SecurityUtilities.HashSHA1(password);
            var validate = await GetSingleAsync(x => x.id == userId && x.password.ToUpper() == password.ToUpper());
            if (validate == null)
                throw new AppException("Mật khẩu không chính xác");
        }
        public async Task ForgotPassword(ForgotPasswordModel model)
        {
            string domainUrl = $"{configuration.GetSection("MySettings:DomainFE").Value}/forgot-password?key=";
            string projectName = configuration.GetSection("MySettings:ProjectName").Value.ToString();
            var user = await GetSingleAsync(x => x.username.ToUpper() == model.username.ToUpper() && x.deleted == false);
            if (user == null)
                throw new AppException("Không tìm thấy tài khoản");
            string keyForgotPassword = Guid.NewGuid().ToString();
            user.keyForgotPassword = keyForgotPassword;
            user.createdDateKeyForgot = Timestamp.Now();
            StringBuilder content = new StringBuilder();
            string title = "Yêu cầu thay đổi mật khẩu";
            content.Append($"<p style=\"color:blue;font-size:30px;\">Đặt lại mật khẩu</p>");
            content.Append($"<p>Chào {user.fullName}</p>");
            content.Append($"<p>Để thực hiện thay đổi mật khẩu bạn vui lòng truy cập <a href=\"{domainUrl}{keyForgotPassword}\">tại đây</a></p>");
            content.Append($"<p>Thông báo từ {projectName}</p>");
            var sendMail = new Thread(async () =>
            {
                await necessaryService.SendMail(new SendMailModel
                {
                    to = user.email,
                    title = title,
                    content = content.ToString()
                });
            });
            sendMail.Start();
            await UpdateAsync(user);
        }
        public async Task ResetPassword(ResetPasswordModel model)
        {
            var user = await GetSingleAsync(x => x.keyForgotPassword == model.key);
            if (user == null)
                throw new AppException("Mã xác nhận không chính xác");
            ///mã tồn tại trong thời gian 30 phút
            var checkTime = Timestamp.Date(DateTime.UtcNow.AddHours(-0.5));
            if (user.createdDateKeyForgot < checkTime)
                throw new AppException("Mã xác nhận đã hết hạn sử dụng");
            user.password = SecurityUtilities.HashSHA1(model.newPassword);
            user.keyForgotPassword = "";
            user.createdDateKeyForgot = 0;
            await UpdateAsync(user);
        }
        public async Task ValidateUser(tbl_Users model)
        {
            if (!string.IsNullOrEmpty(model.code))
            {
                var validateCode = await
                    AnyAsync(x => x.code.ToUpper() == model.code.ToUpper() && x.deleted == false && x.id != model.id);
                if (validateCode)
                    throw new AppException("Mã người dùng đã tồn tại");
            }
            if (!string.IsNullOrEmpty(model.username))
            {
                if (!UserNameFormat(model.username))
                    throw new AppException("Tài khoản đăng nhập không hợp lệ");
                var validateUserName = await
                    AnyAsync(x => x.username.ToUpper() == model.username.ToUpper() && x.deleted == false && x.id != model.id);
                if (validateUserName)
                    throw new AppException("Tên đăng nhập đã tồn tại");
            }
        }
        public bool UserNameFormat(string value)
        {
            string[] arr = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ"," ",};
            foreach (var item in arr)
            {
                if (value.Contains(item))
                    return false;
            }
            return true;
        }
        public async Task<List<AccountModel>> GetAccount()
        {
            var data = await coreDbContext.Set<tbl_Users>().Where(x => x.deleted == false)
                .Select(x => new AccountModel
                {
                    id = x.id,
                    fullName = x.fullName,
                    roles = x.roles,
                    isAdmin = x.isAdmin
                }).ToListAsync();
            return data;
        }
    }
}
