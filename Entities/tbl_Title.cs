using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class tbl_Title : DomainEntities.DomainEntities
    {
        public string name {  get; set; }
        public int quantity {  get; set; }
        public int quantityReal { get; set; }
        public int quantityLoaning { get; set; }
        public int quantityLost { get; set; }
        public string thumbnail {  get; set; }
        public Guid? authorId { get; set; }
        [NotMapped]
        public string authorName {  get; set; }
        /// Chuỗi json chứa categoryId
        public string categoryId { get; set; }
        [NotMapped]
        public string categoryName { get; set; }
    }
}
