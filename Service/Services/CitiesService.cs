using AutoMapper;
using Entities;
using Entities.DomainEntities;
using Interface.Services;
using Interface.UnitOfWork;
using Service.Services.DomainServices;

namespace Service.Services
{
    public class CitiesService : DomainService<tbl_Cities, BaseSearch>, ICitiesService
    {
        public CitiesService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override string GetStoreProcName()
        {
            return "Get_Cities";
        }
    }
}
