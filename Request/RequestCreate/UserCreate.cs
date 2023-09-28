using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using Request.DomainRequests;
using Utilities;
using static Utilities.CoreContants;

namespace Request.RequestCreate
{
    public class UserCreate : DomainCreate
    {
        public string code { get; set; }
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập gồm 50 ký tự")]
        public string username { get; set; }
        /// <summary>
        /// Họ và tên
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [StringLength(200)]
        public string fullName { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        [StringLength(12, ErrorMessage = "Số kí tự của số điện thoại phải lớn hơn 8 và nhỏ hơn 12!", MinimumLength = 9)]
        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]+${9,11}", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string phone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [StringLength(50, ErrorMessage = "Số kí tự của email phải nhỏ hơn 50!")]
        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [EmailAddress(ErrorMessage = "Email có định dạng không hợp lệ!")]
        public string email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [StringLength(1000)]
        public string address { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public double? birthday { get; set; }
        /// <summary>
        /// Giới tính
        /// 1 male - Name <br></br>
        /// 2 female - Nữ <br></br>
        /// 3 other - Khác <br></br>
        /// </summary>
        public gender? gender { get; set; }
        public string thumbnail { get; set; }
        public Guid? districtId { get; set; }
        public Guid? cityId { get; set; }
        public Guid? wardId { get; set; }
        [StringLength(4000)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu đăng nhập")]
        public string password { get; set; }
        //[Required(ErrorMessage = "Vui lòng chọn timezone")]
        //public double timezone { get; set; }
        /// <summary>
        /// Quyền của nhân viên, 
        /// một nhân viên có thể có nhiều quyền, 
        /// mẫu "admin,moderator"
        /// </summary>
        [Required(ErrorMessage = "Không được phép bỏ trống quyền")]
        public string roleCodes { get; set; }
    }
}
