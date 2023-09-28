using BaseAPI.Controllers;
using Entities;
using Entities.Search;
using Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Utilities;

namespace API.Controllers
{
    [Route("api/Title")]
    [ApiController]
    [Description("Đầu sách")]
    [Authorize]
    public class TitleController : BaseController<tbl_Titles, TitleCreate, TitleUpdate, TitleSearch>
    {
        protected ITitleService titleService;
        protected IAuthorService authorService;
        public TitleController
        (
            IServiceProvider serviceProvider,
            ILogger<BaseController<tbl_Titles, TitleCreate, TitleUpdate, TitleSearch>> logger,
            IWebHostEnvironment env
        ) : base(serviceProvider, logger, env)
        {
            this.domainService = serviceProvider.GetRequiredService<ITitleService>();
            this.authorService = serviceProvider.GetRequiredService<IAuthorService>();
        }
        /// <summary>
        /// Thêm mới item
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
                var item = mapper.Map<tbl_Titles>(itemModel);
                await Validate(item);
                if (item != null)
                {
                    var author = await this.authorService.GetByIdAsync(item.authorId.Value);
                    if(author==null)
                        throw new AppException("Không tìm thấy tác giả"); 
                    success = await this.domainService.CreateAsync(item);
                    if (success)
                    {
                        appDomainResult.resultCode = (int)HttpStatusCode.OK; 
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
    }
}
