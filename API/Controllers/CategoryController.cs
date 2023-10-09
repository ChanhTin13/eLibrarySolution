using BaseAPI.Controllers;
using Entities;
using Entities.Search;
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

namespace API.Controllers
{
    [Route("api/Category")]
    [ApiController]
    [Description("Thể loại sách")]
    [Authorize]
    public class CategoryController : BaseController<tbl_Category, CategoryCreate, CategoryUpdate, CategorySearch>
    {
        protected ICategoryService categoryService;
        public CategoryController
        (
            IServiceProvider serviceProvider,
            ILogger<BaseController<tbl_Category, CategoryCreate, CategoryUpdate, CategorySearch>> logger,
            IWebHostEnvironment env
        ) : base(serviceProvider, logger, env)
        {
            this.domainService = serviceProvider.GetRequiredService<ICategoryService>();
        }
    }
}
