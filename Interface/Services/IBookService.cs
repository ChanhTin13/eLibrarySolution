using Entities;
using Entities.Search;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Interface.Services
{
    public interface IBookService:IDomainService<tbl_Book,BookSearch>
    { 
        Task<tbl_Book> GetBookByIBSNAsync(string IBSN);
    }
}
