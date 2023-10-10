using AutoMapper;
using Entities.Search;
using Entities;
using Interface.Services;
using Interface.UnitOfWork;
using Service.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CounterService : DomainService<tbl_Counter, CounterSearch>, ICounterService
    {
        public CounterService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }
}
