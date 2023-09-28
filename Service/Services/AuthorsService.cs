using AutoMapper;
using Entities;
using Entities.DomainEntities;
using Entities.Search;
using Interface.Services;
using Interface.UnitOfWork;
using Service.Services.DomainServices;

namespace Service.Services
{
    public class AuthorsService : DomainService<tbl_Authors, AuthorSearch>, IAuthorService
    {
        public AuthorsService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override string GetStoreProcName()
        {
            return "Get_Authors";
        }
    }
}
