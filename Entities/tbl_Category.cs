using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class tbl_Category:DomainEntities.DomainEntities
    {
        public string name {  get; set; }
        [NotMapped]
        public int totalBook { get; set; }
    }
}
