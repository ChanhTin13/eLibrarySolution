using BaseAPI.Controllers;
using Entities.Search;
using Entities;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [Route("api/Counter")]
    [ApiController]
    [Description("Tác giả")]
    [Authorize]
    public class CounterController : BaseController<tbl_Counter, CounterCreate, CounterUpdate, CounterSearch>
    {
        protected ICounterService counterService;
        public CounterController
        (
            IServiceProvider serviceProvider,
            ILogger<BaseController<tbl_Counter, CounterCreate, CounterUpdate, CounterSearch>> logger,
            IWebHostEnvironment env
        ) : base(serviceProvider, logger, env)
        {
            this.domainService = serviceProvider.GetRequiredService<ICounterService>();
        }
    }
}
