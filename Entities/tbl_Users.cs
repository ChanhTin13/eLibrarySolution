using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Entities
{
    public class tbl_Users : DomainEntities.DomainEntities
    {
        public string code { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        [Required]
        [StringLength(50)]
        public string username { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        [StringLength(200)]
        public string fullName { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        [StringLength(20)]
        public string phone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [StringLength(50)]
        public string email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [StringLength(1000)]
        public string address { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public double? birthday { get; set; }

        /// <summary>
        /// Mật khẩu người dùng
        /// </summary>
        [StringLength(4000)]
        [JsonIgnore]
        public string password { get; set; }

        /// <summary>
        /// </summary>
        public int? gender { get; set; }
        [JsonIgnore]
        public bool? isAdmin { get; set; }
        /// <summary>
        /// [code,code]
        /// </summary>

        [JsonIgnore]
        public string roles { get; set; }
        public string thumbnail { get; set; }
        public Guid? districtId { get; set; }
        [NotMapped]
        public string districtName { get; set; }
        public Guid? cityId { get; set; }
        [NotMapped]
        public string cityName { get; set; }
        public Guid? wardId { get; set; }
        [NotMapped]
        public string wardName { get; set; }
        [JsonIgnore]
        public string keyForgotPassword { get; set; }
        [JsonIgnore]
        public double? createdDateKeyForgot { get; set; }
        [NotMapped]
        public object rolesModel { get { return JsonConvert.DeserializeObject(roles ?? ""); } set { value = new object(); } }
        [JsonIgnore]
        public string oneSignalId { get; set; }
    }
}
