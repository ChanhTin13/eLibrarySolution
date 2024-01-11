using AutoMapper;
using Entities;
using Entities.Search;
using Interface.DbContext;
using Interface.Services;
using Interface.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Utilities;

namespace Service.Services
{
    public class BookService:DomainServices.DomainService<tbl_Book,BookSearch>, IBookService
    { 
        public BookService(IAppUnitOfWork unitOfWork, IMapper mapper ) : base(unitOfWork, mapper)
        { 
        }

        public async Task<tbl_Book> GetBookByIBSNAsync(string IBSN)
        {   
            var data = await GetSingleAsync(x=>x.deleted==false && x.active==true && x.ISBN==IBSN); 
            return data;
        }


    }
}
