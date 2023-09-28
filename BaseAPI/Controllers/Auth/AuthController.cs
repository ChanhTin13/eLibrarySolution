using Entities;
using Extensions;
using Interface.Services;
using Interface.Services.Auth;
using Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Request.Auth;
using Models;
using static Utilities.CoreContants;
using System.ComponentModel;
using Request.RequestUpdate;
using Entities.DomainEntities;

namespace BaseAPI.Controllers.Auth
{
    [ApiController]
    public abstract class AuthController : ControllerBase
    {
        protected readonly ILogger<AuthController> logger;
        protected IUserService userService;
        protected IConfiguration configuration;
        protected IMapper mapper;
        private readonly ITokenManagerService tokenManagerService;
        public AuthController(IServiceProvider serviceProvider
            , IConfiguration configuration
            , IMapper mapper, ILogger<AuthController> logger
            )
        {
            this.logger = logger;
            this.configuration = configuration;
            this.mapper = mapper;

            userService = serviceProvider.GetRequiredService<IUserService>();
            tokenManagerService = serviceProvider.GetRequiredService<ITokenManagerService>();
        }
        [HttpGet("user-info")]
        [AppAuthorize]
        [Description("Lấy thông tin")]
        public virtual async Task<AppDomainResult> GetUserInfo()
        {
            AppDomainResult appDomainResult = new AppDomainResult();

            var user = LoginContext.Instance.CurrentUser;
            var item = await this.userService.GetByIdAsync(user?.userId ?? Guid.Empty);
            if (item != null)
            {
                appDomainResult = new AppDomainResult()
                {
                    success = true,
                    data = item,
                    resultCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                throw new KeyNotFoundException("Item không tồn tại");
            }
            return appDomainResult;
        }

        /// <summary>
        /// Đăng nhập hệ thống
        /// </summary> 
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public virtual async Task<AppDomainResult> LoginAsync([FromForm] Login loginModel)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            bool success = false;
            if (ModelState.IsValid)
            {
                success = await this.userService.Verify(loginModel.username, loginModel.password);
                if (success)
                {
                    var userInfos = await this.userService.GetSingleAsync(e => e.deleted == false
                    && (e.username == loginModel.username));
                    if (userInfos != null)
                    {
                        if (userInfos.status == ((int)userStatus.locked))
                            throw new UnauthorizedAccessException("Tài khoản của bạn đã bị khoá");
                        var token = await GenerateJwtToken(userInfos);
                        appDomainResult = new AppDomainResult()
                        {
                            success = true,
                            data = new
                            {
                                token = token,
                            },
                            resultCode = (int)HttpStatusCode.OK
                        };
                    }
                    else
                        throw new UnauthorizedAccessException("Tên đăng nhập hoặc mật khẩu không chính xác");
                }
                else
                    throw new UnauthorizedAccessException("Tên đăng nhập hoặc mật khẩu không chính xác");
            }
            else
                throw new AppException(ModelState.GetErrorMessage());
            return appDomainResult;
        }
        /// <summary>
        /// đăng nhập cho dev
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="AppException"></exception>
        [AllowAnonymous]
        [HttpPost("login-dev")]
        public virtual async Task<AppDomainResult> LoginDevAsync([FromForm] LoginDev loginModel)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            bool success = false;
            if (ModelState.IsValid)
            {
                success = await this.userService.AnyAsync(x => x.id == loginModel.id && loginModel.key == "m0n4medi4");
                if (success)
                {
                    var userInfos = await this.userService.GetSingleAsync(e => e.deleted == false
                    && e.id == loginModel.id);
                    if (userInfos != null)
                    {
                        if (userInfos.status == ((int)userStatus.locked))
                            throw new UnauthorizedAccessException("Tài khoản của bạn đã bị khoá");
                        var token = await GenerateJwtToken(userInfos);
                        appDomainResult = new AppDomainResult()
                        {
                            success = true,
                            data = new
                            {
                                token = token,
                            },
                            resultCode = (int)HttpStatusCode.OK
                        };

                    }
                }
                else
                    throw new UnauthorizedAccessException("Thông tin không chính xác");
            }
            else
                throw new AppException(ModelState.GetErrorMessage());
            return appDomainResult;
        }
        /// <summary>
        /// Đăng ký
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public virtual async Task<AppDomainResult> Register([FromBody] Register register)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            if (ModelState.IsValid)
            {
                var user = new tbl_Users()
                {
                    fullName = register.fullName,
                    username = register.username,
                    password = register.password,
                    created = Timestamp.Now(),
                    updated = Timestamp.Now(),
                    roles = "[{\"code\":\"student\",\"name\":\"Sinh viên\"}]",
                    active = true,
                    deleted = false,
                    phone = register.phone,
                    email = register.email,
                };
                // Kiểm tra item có tồn tại chưa?
                await this.userService.ValidateUser(user);
                user.password = SecurityUtilities.HashSHA1(register.password);
                appDomainResult.success = await userService.CreateAsync(user);
                appDomainResult.resultCode = (int)HttpStatusCode.OK;
            }
            else
            {
                var resultMessage = ModelState.GetErrorMessage();
                throw new AppException(resultMessage);
            }
            return appDomainResult;
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="changePasswordModel"></param>
        /// <returns></returns>
        [HttpPut("change-password")]
        [Authorize]
        public virtual async Task<AppDomainResult> ChangePassword([FromBody] ChangePassword changePasswordModel)
        {
            try
            {
                AppDomainResult appDomainResult = new AppDomainResult();
                if (ModelState.IsValid)
                {
                    var user = LoginContext.Instance.CurrentUser;
                    if (user == null)
                        throw new AppException("Không phải người dùng hiện tại");
                    await userService.ValidatePassword(user.userId, changePasswordModel.oldPassword);

                    var userInfo = await this.userService.GetByIdAsync(user.userId);
                    string newPassword = SecurityUtilities.HashSHA1(changePasswordModel.newPassword);
                    appDomainResult.success = await userService.UpdateUserPassword(user.userId, newPassword);
                    appDomainResult.resultCode = (int)HttpStatusCode.OK;
                }
                else
                    throw new AppException(ModelState.GetErrorMessage());
                return appDomainResult;
            }
            catch (AppException e)
            {
                throw new AppException(e.Message);
            }
        }
        /// <summary>
        /// Quên mật khẩu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public virtual async Task<AppDomainResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            AppDomainResult appDomainResult = new AppDomainResult();
            await userService.ForgotPassword(model);
            return new AppDomainResult()
            {
                success = true,
                resultCode = (int)HttpStatusCode.OK
            };
        }
        /// <summary>
        /// Tạo mật khẩu mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("reset-password")]
        public virtual async Task<AppDomainResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            AppDomainResult appDomainResult = new AppDomainResult();
            await userService.ResetPassword(model);
            return new AppDomainResult()
            {
                success = true,
                resultCode = (int)HttpStatusCode.OK
            };
        }
        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("logout")]
        public virtual async Task<AppDomainResult> Logout()
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            await this.tokenManagerService.DeactivateCurrentAsync();
            appDomainResult = new AppDomainResult()
            {
                success = true,
                resultCode = (int)HttpStatusCode.OK
            };
            return appDomainResult;
        }
        [HttpPut("update-information")]
        [AppAuthorize]
        public virtual async Task<AppDomainResult> UpdateInformation([FromBody] UserUpdate itemModel)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            bool success = false;
            UserLoginModel user = LoginContext.Instance.CurrentUser;
            itemModel.id = user.userId;
            if (ModelState.IsValid)
            {
                var item = mapper.Map<tbl_Users>(itemModel);
                await this.userService.ValidateUser(item);
                if (item != null)
                {
                    success = await this.userService.UpdateAsync(item);
                    if (success)
                    {
                        appDomainResult.resultCode = (int)HttpStatusCode.OK;
                        appDomainResult.resultMessage = "Thành công";
                    }
                    else
                        throw new Exception("Lỗi trong quá trình xử lý");
                    appDomainResult.success = success;
                }
                else
                    throw new KeyNotFoundException("Item không tồn tại");
            }
            else
                throw new AppException(ModelState.GetErrorMessage());
            return appDomainResult;
        }
        [AllowAnonymous]
        [HttpGet("account")]
        public virtual async Task<AppDomainResult> GetAccount()
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            var data = await userService.GetAccount();
            return new AppDomainResult()
            {
                data = data,
                success = true,
                resultCode = (int)HttpStatusCode.OK
            };
        }

        /// <summary>
        /// Cập nhật id one signal cho user
        /// </summary>
        /// <param name="oneSignalId"></param>
        /// <returns></returns>
        [HttpPut("update-onesignal-id")]
        [AppAuthorize]
        [Description("Cập nhật id onesignal")]
        public virtual async Task<AppDomainResult> UpdateOneSignalId([FromBody] string oneSignalId)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            if (string.IsNullOrEmpty(oneSignalId))
                throw new AppException("Vui lòng nhập mã one signal");
            var userLogin = LoginContext.Instance.CurrentUser;
            if (userLogin == null)
                throw new AppException("Vui lòng đăng nhập");
            var user = await userService.GetByIdAsync(userLogin.userId);
            if (user == null)
                throw new AppException("Tài khoản không tồn tại");
            user.oneSignalId = oneSignalId;
            bool success = await userService.UpdateFieldAsync(user, x => x.oneSignalId);
            return new AppDomainResult()
            {
                resultCode = (int)HttpStatusCode.OK,
                resultMessage = "Cập nhật thành công!",
                success = success
            };
        }
        #region Private methods

        /// <summary>
        /// Tạo token từ thông tin user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [NonAction]
        protected async Task<string> GenerateJwtToken(tbl_Users user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var appSettingsSection = configuration.GetSection("AppSettings");
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.secret);

            var userLoginModel = new UserLoginModel()
            {
                userId = user.id,
                userName = user.username,
                fullName = user.fullName,
                email = user.email,
                phone = user.phone,
                thumbnail = user.thumbnail,
                isAdmin = user.isAdmin.Value,
                roles = user.roles,
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(userLoginModel))
                            }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}
