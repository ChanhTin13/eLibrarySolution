using BaseAPI.Controllers;
using Entities.Search;
using Entities;
using Interface.DbContext;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Request.RequestCreate;
using Request.RequestUpdate;
using System.ComponentModel;
using System;
using Microsoft.Extensions.DependencyInjection;
using Extensions;
using Service.Services;
using System.Threading.Tasks;
using System.Net;
using Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    [Route("api/LibraryLoan")]
    [ApiController]
    [Description("Tác giả")]
    [Authorize]
    public class LibraryLoanController : BaseController<tbl_LibraryLoan,
                                                 LibraryLoanCreate,
                                                 LibraryLoanUpdate,
                                                 LibraryLoanSearch>
    {
        protected ILibraryLoanService libraryLoanService;
        protected IBookService bookService;
        protected IAppDbContext appDbContext;
        public LibraryLoanController
        (
            IServiceProvider serviceProvider,
            ILogger<BaseController<tbl_LibraryLoan,
                                                 LibraryLoanCreate,
                                                 LibraryLoanUpdate,
                                                 LibraryLoanSearch>> logger,
            IWebHostEnvironment env
        ) : base(serviceProvider, logger, env)
        {
            this.domainService = serviceProvider.GetRequiredService<ILibraryLoanService>();
            this.bookService = serviceProvider.GetRequiredService<IBookService>();
            this.appDbContext = serviceProvider.GetRequiredService<IAppDbContext>();
        }


        public override async Task Validate(tbl_LibraryLoan model)
        {
            var item = await domainService.GetByIdAsync(model.id);
            if (item == null)
                throw new AppException(ApiMessage.ItemNotFound);
        }
        /// <summary>
        /// mượn sách
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới")]
        public override async Task<AppDomainResult> AddItem([FromBody] LibraryLoanCreate itemModel)
        {
            using (var tran = await appDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    AppDomainResult appDomainResult = new AppDomainResult();
                    bool success = false;
                    if (!ModelState.IsValid)
                        throw new AppException(ModelState.GetErrorMessage());
                    //kiểm tra sách mượn
                    foreach (var bookLoan in itemModel.listBookLoan)
                    {
                        var book = await bookService.GetByIdAsync(bookLoan.bookId);
                        if (book == null)
                        {
                            throw new AppException($"Không tìm thấy sách {book.ISBN}");
                        }
                        if (book.status == true)
                        {
                            throw new AppException($"{book.ISBN} đang được mượn");
                        }
                    }
                    var item = mapper.Map<tbl_LibraryLoan>(itemModel);
                    if (item != null)
                    {
                        item.code = "code " + Timestamp.Now().ToString();
                        item.totalBook = itemModel.listBookLoan.Count;
                        item.listBookLoan = JsonConvert.SerializeObject(itemModel.listBookLoan);
                        // tạo phiếu 
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
                    await tran.CommitAsync();
                    return appDomainResult;
                }
                catch (AppException e)
                {
                    await tran.RollbackAsync();
                    throw new AppException(e.Message);
                }
            }
        }
        /// <summary>
        /// Trả sách
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [AppAuthorize]
        [Description("Trả sách")]
        public override async Task<AppDomainResult> UpdateItem([FromBody] LibraryLoanUpdate itemModel)
        {
            using (var tran = await appDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    AppDomainResult appDomainResult = new AppDomainResult();
                    bool success = false;
                    if (ModelState.IsValid)
                    {
                        //var item = mapper.Map<tbl_LibraryLoan>(itemModel);
                        //if (item == null)
                        //    throw new KeyNotFoundException("Item không tồn tại");
                        // tìm phiếu mượn 
                        var item = await domainService.GetByIdAsync(itemModel.id);
                        if (item == null)
                            throw new AppException("Phiếu mượn sách không tồn tại"); 

                        // kiểm tra sách
                        var listBookLoan = JsonConvert.DeserializeObject<List<LibraryLoanDetail>>(item.listBookLoan);
                        var LibraryLoanDetail = listBookLoan.FirstOrDefault(b=>b.bookId==itemModel.bookId.Value || b.IBSN==itemModel.IBSN);
                        if (LibraryLoanDetail == null)
                            throw new AppException("Không tìm thấy sách");
                        //trả sách  
                        LibraryLoanDetail.isReturned = true;
                        LibraryLoanDetail.dateReturn = Timestamp.Now();
                        // kiểm tra đã trả hết sách chưa
                        var isReturnAll= listBookLoan.All(x=>x.isReturned==true);
                        if (isReturnAll == true)
                            item.isReturnAll = true;
                        // update
                        success = await this.domainService.UpdateAsync(item);
                        if (success) 
                            appDomainResult.resultCode = (int)HttpStatusCode.OK; 
                        else
                            throw new Exception("Lỗi trong quá trình xử lý");
                        appDomainResult.success = success;
                    }
                    else
                        throw new AppException(ModelState.GetErrorMessage());
                    await tran.CommitAsync();
                    return appDomainResult;
                }
                catch (AppException e)
                {
                    await tran.RollbackAsync();
                    throw new AppException(e.Message);
                }
            }
        }
        /// <summary>
        /// Gia hạn
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("renewal")]
        [AppAuthorize]
        [Description("Gia hạn")]
        public async Task<AppDomainResult> Renewal([FromBody] LibraryLoanUpdate itemModel)
        {
            using (var tran = await appDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    AppDomainResult appDomainResult = new AppDomainResult();
                    bool success = false;
                    if (ModelState.IsValid)
                    {
                        //var item = mapper.Map<tbl_LibraryLoan>(itemModel);
                        //if (item == null)
                        //    throw new KeyNotFoundException("Item không tồn tại");
                        // tìm phiếu mượn 
                        var item = await domainService.GetByIdAsync(itemModel.id);
                        if (item == null)
                            throw new AppException("Phiếu mượn sách không tồn tại");

                        // kiểm tra sách
                        var listBookLoan = JsonConvert.DeserializeObject<List<LibraryLoanDetail>>(item.listBookLoan);
                        var LibraryLoanDetail = listBookLoan.FirstOrDefault(b => b.bookId == itemModel.bookId.Value || b.IBSN == itemModel.IBSN);
                        if (LibraryLoanDetail == null)
                            throw new AppException("Không tìm thấy sách");
                        // check điều kiện gia hạn

                        //gia hạn  thêm 3d
                        double timeAdd = 3 * 24 * 60;
                        LibraryLoanDetail.dateExpire = Timestamp.AddMinutes( LibraryLoanDetail.dateExpire, timeAdd);
                        LibraryLoanDetail.renewals = LibraryLoanDetail.renewals+1; 
                        // update
                        success = await this.domainService.UpdateAsync(item);
                        if (success)
                            appDomainResult.resultCode = (int)HttpStatusCode.OK;
                        else
                            throw new Exception("Lỗi trong quá trình xử lý");
                        appDomainResult.success = success;
                    }
                    else
                        throw new AppException(ModelState.GetErrorMessage());
                    await tran.CommitAsync();
                    return appDomainResult;
                }
                catch (AppException e)
                {
                    await tran.RollbackAsync();
                    throw new AppException(e.Message);
                }
            }
        }

    }
}
