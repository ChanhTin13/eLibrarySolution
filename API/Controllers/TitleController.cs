using BaseAPI.Controllers;
using Entities;
using Entities.Search;
using Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Request.RequestCreate;
using Request.RequestUpdate;
using Service.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Utilities;

namespace API.Controllers
{
    [Route("api/Title")]
    [ApiController]
    [Description("Đầu sách")]
    [Authorize]
    public class TitleController : BaseController<tbl_Title, TitleCreate, TitleUpdate, TitleSearch>
    {
        protected ITitleService titleService;
        protected ICategoryService categoryService;
        protected IAuthorService authorService;
        public TitleController
        (
            IServiceProvider serviceProvider,
            ILogger<BaseController<tbl_Title, TitleCreate, TitleUpdate, TitleSearch>> logger,
            IWebHostEnvironment env
        ) : base(serviceProvider, logger, env)
        {
            this.domainService = serviceProvider.GetRequiredService<ITitleService>();
            this.authorService = serviceProvider.GetRequiredService<IAuthorService>();
            this.categoryService = serviceProvider.GetRequiredService<ICategoryService>();
        }
         


        public async override Task<AppDomainResult> GetById(Guid id)
        {

            var item = await this.domainService.GetByIdAsync(id);
            if (item != null)
            {
                string listName = "";
                var categoryList = item.categoryId.Split(',');
                foreach (var category in categoryList)
                {
                    var c = await categoryService.GetSingleAsync(x => x.id.ToString() == category.Trim());
                    if (c != null)
                    {
                        listName = listName + c.name + ",";
                    }
                }
                item.categoryName = listName == "" ? listName : listName.Remove(listName.Length - 1);
                // tác giả
                var author = await authorService.GetByIdAsync(item.authorId ?? Guid.Empty);
                if (author != null) { item.authorName = author.name; }
                return new AppDomainResult() { success = true, data = item, resultCode = (int)HttpStatusCode.OK };
            }
            throw new KeyNotFoundException(ApiMessage.ItemNotFound);
        }
        /// <summary>
        /// Thêm mới đầu sách
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới")]
        public override async Task<AppDomainResult> AddItem([FromBody] TitleCreate itemModel)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            bool success = false;
            if (ModelState.IsValid)
            {

                if (itemModel.categoryId != null)
                {
                    var categoryList = itemModel.categoryId.Split(',');
                    foreach (var category in categoryList)
                    {
                        if (Guid.TryParse(category.Trim(), out _) != true)
                            throw new AppException("Category string is not in the correct format");
                    }
                }
                var item = mapper.Map<tbl_Title>(itemModel);
                await Validate(item);
                if (item != null)
                {
                    //var author = await this.authorService.GetByIdAsync(item.authorId.Value);
                    //if(author==null)
                    //    throw new AppException("Không tìm thấy tác giả");  
                     
                    item.quantityReal = itemModel.quantity;
                    success = await this.domainService.CreateAsync(item);
                    if (success)
                    {
                        appDomainResult.resultCode = (int)HttpStatusCode.OK;
                    }
                    else
                        throw new Exception(ApiMessage.HandlingError);
                    appDomainResult.success = success;
                }
                else
                    throw new AppException(ApiMessage.ItemNotFound);
            }
            else
            {
                throw new AppException(ModelState.GetErrorMessage());
            }
            return appDomainResult;
        }

        public override async Task<AppDomainResult> UpdateItem([FromBody] TitleUpdate itemModel)
        { 
            if (ModelState.IsValid)
            {
                if (itemModel.categoryId != null)
                {
                    var categoryList = itemModel.categoryId.Split(',');
                    foreach (var category in categoryList)
                    {
                        if (Guid.TryParse(category.Trim(), out _) != true)
                            throw new AppException("Category string is not in the correct format");
                    }
                }
                var item = await domainService.GetByIdAsync(itemModel.id);
                if (item != null)
                {
                    item.name = itemModel.name;
                    item.authorId = itemModel.authorId;
                    item.categoryId = itemModel.categoryId;
                    AppDomainResult appDomainResult = new AppDomainResult();
                    bool success = false;
                    success = await this.domainService.UpdateAsync(item);

                    if (success)
                    {
                        appDomainResult.resultCode = (int)HttpStatusCode.OK;
                    }
                    else
                        throw new Exception(ApiMessage.HandlingError);
                    appDomainResult.success = success;
                    return appDomainResult;
                }
                else
                    throw new AppException(ApiMessage.ItemNotFound);
            }
            else
            {
                throw new AppException(ModelState.GetErrorMessage());
            }
        }

        //Import Image
        /// <summary>
        /// upload 1 image
        /// </summary>
        /// <param name="thumnail"></param>
        /// <param name="titleId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="AppException"></exception>
        [HttpPost("upload-title-thumnails")]
        [AppAuthorize]
        [Description("Thêm ảnh cho sách")]
        public async Task<AppDomainResult> UploadFileImage(IFormFile thumnail, Guid titleId)
        {
            AppDomainResult appDomainResult = new AppDomainResult();

            await Task.Run(async () =>
            {
                var title = await this.domainService.GetByIdAsync(titleId);
                if (title == null) throw new Exception("Không tìm thấy thông tin sách !");

                if (thumnail != null && thumnail.Length > 0)
                {
                    //check if thumnail is image
                    string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".jfif", ".tiff", ".webp", ".ico" };
                    string extension = Path.GetExtension(thumnail.FileName).ToLowerInvariant();
                    var isImage= allowedImageExtensions.Contains(extension);
                    if(!isImage) { throw new AppException(ApiMessage.InvalidFile); }
                    // save thumnail
                    string fileName = string.Format("{0}-{1}", Guid.NewGuid().ToString(), thumnail.FileName);
                    string fileUploadPath = Path.Combine(env.ContentRootPath, CoreContants.TITLE_FOLDER_NAME);
                    string path = Path.Combine(fileUploadPath, fileName);
                    FileUtilities.CreateDirectory(fileUploadPath);
                    var fileByte = FileUtilities.StreamToByte(thumnail.OpenReadStream());
                    FileUtilities.SaveToPath(path, fileByte); 
                    //update title thumnail
                    title.thumbnail = path;
                    await domainService.UpdateAsync(title);
                    appDomainResult = new AppDomainResult()
                    {
                        success = true,
                        data = title,
                        resultMessage = "Upload hình ảnh thành công!",
                        resultCode = (int)HttpStatusCode.OK
                    };
                }
            });
            return appDomainResult;
        }
    }
}
