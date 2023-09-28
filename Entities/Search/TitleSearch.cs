using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Search
{
    public class TitleSearch:BaseSearch
    {
        public string name { get; set; }
        public Guid? authorId { get; set; }
        /// <summary>
        /// ví dụ: 1,2,3
        /// </summary>
        public string categoryId { get; set; }
    }
}
