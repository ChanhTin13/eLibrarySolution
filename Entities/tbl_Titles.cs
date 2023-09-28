using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class tbl_Titles : DomainEntities.DomainEntities
    {
        public string name {  get; set; }
        public int quantity {  get; set; }
        public int quantityReal { get; set; }
        public int quantityLoaning { get; set; }
        public int quantityLost { get; set; }
        public string thumbnail {  get; set; }
        public Guid? authorId { get; set; }
        public string categoryId { get; set; }
    }
}
