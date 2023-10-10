using Entities;
using Entities.Search;
using Interface.Services.DomainServices;

namespace Interface.Services
{
    public interface ICounterService:IDomainService<tbl_Counter,CounterSearch>
    {
    }
}
