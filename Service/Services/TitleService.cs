using Entities.Search;
using Entities;
using Interface.Services;
using Service.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Interface.UnitOfWork;

namespace Service.Services
{
    public class TitleService : DomainService<tbl_Titles, TitleSearch>, ITitleService
    {

        public TitleService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public Task<object> UpdateQuantityLost(Guid titleId)
        {
            throw new NotImplementedException();
        }

        public Task<object> UpdateThumbnail(Guid titleId)
        {
            throw new NotImplementedException();
        }

        protected override string GetStoreProcName()
        {
            return "Get_Titles";
        }
    }
}
