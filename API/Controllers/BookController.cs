using AppDbContext;
using BaseAPI.Controllers;
using Entities;
using Entities.Search;
using Extensions;
using Interface.DbContext;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Request.RequestCreate;
using Request.RequestUpdate;
using Service.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CoreContants;

namespace API.Controllers
{
    [Route("api/Book")]
    [ApiController]
    [Description("Tác giả")]
    [Authorize]
    public class BookController : BaseController<tbl_Book, 
                                                 BookCreate,
                                                 BookUpdate,
                                                 BookSearch>
    {
        protected IBookService bookService;
        protected ITitleService titleService; 
        protected IAppDbContext appDbContext;
        public BookController
        (
            IServiceProvider serviceProvider,
            ILogger<BaseController<tbl_Book,
                                    BookCreate,
                                    BookUpdate,
                                    BookSearch>> logger,
            IWebHostEnvironment env
        ) : base(serviceProvider, logger, env)
        {
            this.domainService = serviceProvider.GetRequiredService<IBookService>();
            this.bookService = serviceProvider.GetRequiredService<IBookService>();
            this.titleService = serviceProvider.GetRequiredService<ITitleService>();
            this.titleService = serviceProvider.GetRequiredService<ITitleService>();
            this.appDbContext = serviceProvider.GetRequiredService<IAppDbContext>();
        }



        public override async Task Validate(tbl_Book model)
        {
            var title = await titleService.GetByIdAsync(model.titleId.Value);
            if (title == null)
                throw new AppException("Đầu sách không tồn tại");
        }
        /// <summary>
        /// Thêm mới item
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới")]
        public override async Task<AppDomainResult> AddItem([FromBody] BookCreate itemModel)
        {
            using (var tran = await appDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!ModelState.IsValid)
                        throw new AppException(ModelState.GetErrorMessage());

                    var item = mapper.Map<tbl_Book>(itemModel);
                    if (item == null)
                        throw new AppException("Item không tồn tại");
                    // kiểm tra đầu sách
                    await Validate(item);

                    await this.domainService.CreateAsync(item);  
                    await tran.CommitAsync();
                }
                catch (AppException e)
                {
                    await tran.RollbackAsync();
                    throw new AppException(e.Message);
                }
            }
            return new AppDomainResult
            {
                success = true,
                resultCode = (int)HttpStatusCode.OK
            };
        }

        /// <summary>
        /// Cập nhật thông tin item
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [AppAuthorize]
        [Description("Chỉnh sửa")]
        public override async Task<AppDomainResult> UpdateItem([FromBody] BookUpdate itemModel)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            bool success = false;
            if (ModelState.IsValid)
            {
                var item = mapper.Map<tbl_Book>(itemModel);
                if (item != null)
                {  
                    success = await this.domainService.UpdateAsync(item);
                    if (success)
                        appDomainResult.resultCode = (int)HttpStatusCode.OK;
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

        [HttpGet("{IBSN}")]
        [AppAuthorize]
        [Description("Lấy thông tin sách theo IBSN")]
        public async Task<AppDomainResult> GetBookByIBSN(string IBSN)
        {
            AppDomainResult appDomainResult = new AppDomainResult(); 
            var item = await this.bookService.GetBookByIBSNAsync(IBSN);
            if (item != null)
            {

                var title = await titleService.GetByIdAsync(item.titleId.Value);
                //var counter = await coun.GetByIdAsync(item.examId.Value);
                item.titleName = title?.name;
                //item.counterName = counter?.name;
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

    }
}
