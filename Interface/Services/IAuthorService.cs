using Entities.DomainEntities;
using Entities;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Search;

namespace Interface.Services
{
    public interface IAuthorService : IDomainService<tbl_Authors, AuthorSearch>
    {
    }
}
