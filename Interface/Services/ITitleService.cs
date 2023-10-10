using Entities;
using Entities.Search;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services
{
    public interface ITitleService:IDomainService<tbl_Title,TitleSearch>
    {
        Task<object> UpdateThumbnail(Guid titleId);
        Task<object> UpdateQuantityLost(Guid titleId);
    }
}
