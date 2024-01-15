using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.DomainRequests
{
    public class DomainModel
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public double? created { get; set; }

        /// <summary>
        /// Tạo bởi
        /// </summary>
        public Guid? createdBy { get; set; }

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public double? updated { get; set; }

        /// <summary>
        /// Người cập nhật
        /// </summary>
        public Guid? updatedBy { get; set; }

        /// <summary>
        /// Cờ active
        /// </summary>
        [DefaultValue(true)]
        public bool? active { get; set; }
    }
}
