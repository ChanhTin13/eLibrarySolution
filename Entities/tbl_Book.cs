using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class tbl_Book : DomainEntities.DomainEntities
    {
        public string ISBN {  get; set; }
        /// <summary>
        /// 0: chưa mượn
        /// 1: đang mượn
        /// </summary>
        public bool status {  get; set; }
        public int counterId {  get; set; }
        [NotMapped]
        public string counterName { get; set; }
        public Guid? titleId { get; set; }
        [NotMapped]
        public string titleName { get; set; }
        /// <summary>
        /// 0: còn sách
        /// 1: mất sách
        /// </summary>
        public bool isLost { get; set; } = false;
    }
}
