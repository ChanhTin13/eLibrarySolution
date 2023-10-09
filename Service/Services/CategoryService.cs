using AutoMapper;
using Entities;
using Entities.Search;
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
    public class CategoryService:DomainService<tbl_Category,CategorySearch>,ICategoryService
    {
        public CategoryService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override string GetStoreProcName()
        {
            return "Get_Categories";
        }
    }
}
