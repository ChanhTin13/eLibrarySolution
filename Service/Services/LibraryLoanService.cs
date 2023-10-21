using AutoMapper;
using Entities.Search;
using Entities;
using Interface.DbContext;
using Interface.Services;
using Interface.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Request.RequestCreate;

namespace Service.Services
{
    public class LibraryLoanService : DomainServices.DomainService<tbl_LibraryLoan, LibraryLoanSearch>, ILibraryLoanService
    {
        private IAppDbContext appDbContext;
        public LibraryLoanService(IAppUnitOfWork unitOfWork, IMapper mapper, IAppDbContext appDbContext) : base(unitOfWork, mapper)
        {
            this.appDbContext = appDbContext;
        }  
    }
}
