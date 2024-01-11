using Microsoft.AspNetCore.Http;
using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestCreate
{
    public class TitleCreate:DomainCreate
    {
        public string name { get; set; }
        public int quantity { get; set; } = 1;
        //public int quantityReal { get; set; }
        //public int quantityLoaning { get; set; }=0;
        //public int quantityLost { get; set; } = 0; 
        public Guid? authorId { get; set; } = null;
        /// <summary>
        /// ví dụ: 1,2,3
        /// </summary>
        public string categoryId { get; set; } = new Guid().ToString(); 
    }
}
