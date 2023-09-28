using Entities.DomainEntities;
using Extensions;
using Interface;
using Interface.Services;
using Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Interface.Services.DomainServices;
using Request.DomainRequests;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Interface.UnitOfWork;

namespace BaseAPI.Controllers
{
    [ApiController]
    public abstract class BaseController<E, C, U, F> : ControllerBase
        where E : Entities.DomainEntities.DomainEntities 
        where C : DomainCreate 
        where U : DomainUpdate
        where F : BaseSearch, new()
    {
        protected readonly ILogger<BaseController<E, C, U, F>> logger;
        protected readonly IServiceProvider serviceProvider;
        protected readonly IMapper mapper;
        protected IDomainService<E, F> domainService;
        protected IWebHostEnvironment env;
        private readonly IUserService userService;

        public BaseController(IServiceProvider serviceProvider, ILogger<BaseController<E, C, U, F>> logger, IWebHostEnvironment env)
        {
            this.env = env;
            this.logger = logger;
            this.mapper = serviceProvider.GetService<IMapper>();
            this.serviceProvider = serviceProvider;
            userService = serviceProvider.GetRequiredService<IUserService>();
        }

        /// <summary>
        /// Lấy thông tin theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AppAuthorize]
        [Description("Lấy thông tin")]
        public virtual async Task<AppDomainResult> GetById(Guid id)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            var item = await this.domainService.GetByIdAsync(id);
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
        /// Thêm mới item
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới")]
        public virtual async Task<AppDomainResult> AddItem([FromBody] C itemModel)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            bool success = false;
            if (ModelState.IsValid)
            {
                var item = mapper.Map<E>(itemModel);
                await Validate(item);
                if (item != null)
                {
                    success = await this.domainService.CreateAsync(item);
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
                    throw new AppException("Item không tồn tại");
            }
            else
            {
                throw new AppException(ModelState.GetErrorMessage());
            }
            return appDomainResult;
        }

        /// <summary>
        /// Cập nhật thông tin item
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [AppAuthorize]
        [Description("Chỉnh sửa")]
        public virtual async Task<AppDomainResult> UpdateItem([FromBody] U itemModel)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            bool success = false;
            if (ModelState.IsValid)
            {
                var item = mapper.Map<E>(itemModel);
                await Validate(item);
                if (item != null)
                {
                    success = await this.domainService.UpdateAsync(item);
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

        /// <summary>
        /// Xóa item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AppAuthorize]
        [Description("Xoá")]
        public virtual async Task<AppDomainResult> DeleteItem(Guid id)
        {
            AppDomainResult appDomainResult = new AppDomainResult();

            bool success = await this.domainService.DeleteAsync(id);
            if (success)
            {
                appDomainResult.resultCode = (int)HttpStatusCode.OK;
                appDomainResult.resultMessage = "Thành công";
                appDomainResult.success = success;
            }
            else
                throw new Exception("Lỗi trong quá trình xử lý");

            return appDomainResult;
        }

        /// <summary>
        /// Lấy danh sách item phân trang
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Lấy danh sách")]
        public virtual async Task<AppDomainResult> Get([FromQuery] F baseSearch)
        {
            if (ModelState.IsValid)
            {
                PagedList<E> pagedData = await this.domainService.GetPagedListData(baseSearch);
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
        [NonAction]
        public virtual async Task Validate(E model) {
            await Task.Run(() => { });
        }
        //[HttpPost("upload-file")]
        //[AppAuthorize( CoreContants.Post )]
        //public virtual async Task<AppDomainResult> UploadFile(IFormFile file)
        //{
        //    AppDomainResult appDomainResult = new AppDomainResult();

        //    await Task.Run(() =>
        //    {
        //        if (file != null && file.Length > 0)
        //        {
        //            string fileName = string.Format("{0}-{1}", Guid.NewGuid().ToString(), file.FileName);
        //            string fileUploadPath = Path.Combine(env.ContentRootPath, CoreContants.UPLOAD_FOLDER_NAME, CoreContants.TEMP_FOLDER_NAME);
        //            string path = Path.Combine(fileUploadPath, fileName);
        //            FileUtilities.CreateDirectory(fileUploadPath);
        //            var fileByte = FileUtilities.StreamToByte(file.OpenReadStream());
        //            FileUtilities.SaveToPath(path, fileByte);
        //            appDomainResult = new AppDomainResult()
        //            {
        //                success = true,
        //                data = fileName
        //            };
        //        }
        //    });
        //    return appDomainResult;
        //}
        [NonAction]
        public async Task<string> UploadFile(IFormFile file, string folder)
        {
            var httpContextHost = HttpContext.Request.Host;
            string result = "";
            await Task.Run(() =>
            {
                if (file != null && file.Length > 0)
                {
                    string fileName = string.Format("{0}-{1}", Guid.NewGuid().ToString(), file.FileName);
                    string fileUploadPath = Path.Combine(env.ContentRootPath, CoreContants.UPLOAD_FOLDER_NAME, folder);
                    string path = Path.Combine(fileUploadPath, fileName);
                    FileUtilities.CreateDirectory(fileUploadPath);
                    var fileByte = FileUtilities.StreamToByte(file.OpenReadStream());
                    FileUtilities.SaveToPath(path, fileByte);
                    result = $"https://{httpContextHost}/{folder}/{fileName}";
                }
                else
                {
                    throw new AppException("Không tìm thấy tệp");
                }
            });
            return result;
        }
    }
}
