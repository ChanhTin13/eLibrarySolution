using Request.DomainRequests;
using System.ComponentModel.DataAnnotations;

namespace Request.RequestCreate
{
    public class AuthorCreate : DomainCreate
    {
        /// <summary>
        /// Tên tác giả
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập tên tác giả")]
        [StringLength(50, ErrorMessage = "Tên tác giả gồm 50 ký tự")]
        public string name {  get; set; }
    }
}
