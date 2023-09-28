using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using static Utilities.CoreContants;
namespace Entities.Search
{
    public class UserSearch : BaseSearch
    {
        /// <summary>
        /// mẫu: "code,code"
        /// </summary>
        public string roleCodes { get; set; }
        public int gender { get; set; } = 0;
        /// <summary>
        /// 0 - Giảm dần
        /// 1 - Tăng gần
        /// 2 - Tên tăng dần
        /// 3 - Tên giảm dần
        /// </summary>
        [DefaultValue(0)]
        public override int orderBy { set; get; }
    }
    public class StudentSearchPrivate : UserSearch
    {
        /// <summary>
        /// lấy học viên
        /// </summary>
        public int type { get { return 1; } }
    }
    public class StaffSearchPrivate : UserSearch
    {
        /// <summary>
        /// lấy nhân viên
        /// </summary>
        public int type { get { return 2; } }
    }
    public class UserSearchByGeneralNotification
    {
        /// <summary>
        /// roleCodes : value1,value2
        /// </summary>
        public string roleCodes { get; set; }
        /// <summary>
        /// majorsId : value1,value2
        /// </summary>
        public string majorsId { get; set; }
    }
}
