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
    [Route("api/Author")]
    [ApiController]
    [Description("Tác giả")]
    [Authorize]
    public class AuthorController : BaseController<tbl_Authors, AuthorCreate, AuthorUpdate, AuthorSearch>
    {
        protected IAuthorService AuthorService;
        public AuthorController
        (
            IServiceProvider serviceProvider,
            ILogger<BaseController<tbl_Authors, AuthorCreate, AuthorUpdate, AuthorSearch>> logger,
            IWebHostEnvironment env
        ) : base(serviceProvider, logger, env)
        {
            this.domainService = serviceProvider.GetRequiredService<IAuthorService>();
        }
    }
}
