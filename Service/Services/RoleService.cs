using AutoMapper;
using Entities;
using Entities.DomainEntities;
using Interface.Services;
using Interface.UnitOfWork;
using Service.Services.DomainServices;

namespace Service.Services
{
    public class RoleService : DomainService<tbl_Role, BaseSearch>, IRoleService
    {
        public RoleService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }
}
