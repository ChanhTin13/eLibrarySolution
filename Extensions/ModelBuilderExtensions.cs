using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using static Utilities.CoreContants;
using Utilities;

namespace Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var userId = Guid.NewGuid();
            modelBuilder.Entity<tbl_Users>().HasData(
                    new tbl_Users()
                    {
                        id = userId,
                        username = "admin",
                        fullName = "Mona Media",
                        phone = "1900 636 648",
                        email = "info@mona-media.com",
                        address = "373/226 Lý Thường Kiệt, P8, Q. Tân Bình, HCM",
                        status = ((int)userStatus.active),
                        birthday = 0,
                        password = SecurityUtilities.HashSHA1("daihocyduoc"),
                        gender = 0,
                        roles = "[{\"code\":\"admin\",\"name\":\"Quản trị viên\"}]",
                        isAdmin = true,
                        code = "AD-000001",
                        created = Timestamp.Now(),
                        updated = Timestamp.Now(),
                        createdBy = userId,
                        updatedBy = userId,
                        deleted = false,
                        active = true
                    }
                );
            //modelBuilder.Entity<tbl_Necessary>().HasData(
            //        new tbl_Necessary()
            //        {
            //            id = Guid.NewGuid(),
            //            type = 4,
            //            typeName = "Hình thức thanh toán",
            //            content = "[{\"id\":\"ee6e8181-8234-41f9-bacb-306c037c7105\",\"name\":\"Chuyển khoản\",\"code\":\"chuyen-khoan\",\"note\":\"\"}," +
            //            "{\"id\":\"c698997e-c6d4-45c3-8b5a-114e5e5b78aa\",\"name\":\"Tiền mặt\",\"code\":\"tien-mat\",\"note\":\"\"}]",
            //            created = Timestamp.Now(),
            //            updated = Timestamp.Now(),
            //            createdBy = userId,
            //            updatedBy = userId,
            //            deleted = false,
            //            active = true
            //        },
            //        new tbl_Necessary()
            //        {
            //            id = Guid.NewGuid(),
            //            type = 9,
            //            typeName = "Mẫu",
            //            content = "[{\"content\":\"\",\"type\":1,\"typeName\":\"Phiếu thu\",\"active\":true}," +
            //            "{\"content\":\"\",\"type\":2,\"typeName\":\"Phiếu chi\",\"active\":true}," +
            //            "{\"content\":\"\",\"type\":3,\"typeName\":\"Thông báo hẹn kiểm tra năng lực\",\"active\":true}," +
            //            "{\"content\":\"\",\"type\":4,\"typeName\":\"Thông báo tạo tài khoản thành công\",\"active\":true}," +
            //            "{\"content\":\"\",\"type\":5,\"typeName\":\"Thông báo sắp tới giờ học\",\"active\":true}," +
            //            "{\"content\":\"\",\"type\":6,\"typeName\":\"Thông báo nhập học\",\"active\":true}," +
            //            "{\"content\":\"\",\"type\":7,\"typeName\":\"Thông báo thay đổi lịch dạy giáo viên\",\"active\":true}," +
            //            "{\"content\":\"\",\"type\":8,\"typeName\":\"Thông báo thay đổi lịch học cho học viên\",\"active\":true}," +
            //            "{\"content\":\"\",\"type\":9,\"typeName\":\"Thông báo có bài tập mới\",\"active\":true}," +
            //            "{\"content\":\"\",\"type\":10,\"typeName\":\"Thông báo khai giảng khoá học mới cho giáo viên\",\"active\":true}," +
            //            "{\"content\":\"\",\"type\":11,\"typeName\":\"Thông báo mua khoá học video thành công\",\"active\":true}]",
            //            created = Timestamp.Now(),
            //            updated = Timestamp.Now(),
            //            createdBy = userId,
            //            updatedBy = userId,
            //            deleted = false,
            //            active = true
            //        }
            //    );
            //modelBuilder.Entity<tbl_Role>().HasData(
            //        new tbl_Role()
            //        {
            //            id = Guid.NewGuid(),
            //            name = "Admin",
            //            code = "admin",
            //            permissions = "",
            //            created = Timestamp.Now(),
            //            updated = Timestamp.Now(),
            //            createdBy = userId,
            //            updatedBy = userId,
            //            deleted = false,
            //            active = true
            //        },
            //        new tbl_Role()
            //        {
            //            id = Guid.NewGuid(),
            //            name = "Quản lý",
            //            code = "manager",
            //            permissions = "",
            //            created = Timestamp.Now(),
            //            updated = Timestamp.Now(),
            //            createdBy = userId,
            //            updatedBy = userId,
            //            deleted = false,
            //            active = true
            //        },
            //        new tbl_Role()
            //        {
            //            id = Guid.NewGuid(),
            //            name = "Nhân viên kinh doanh",
            //            code = "saler",
            //            permissions = "",
            //            created = Timestamp.Now(),
            //            updated = Timestamp.Now(),
            //            createdBy = userId,
            //            updatedBy = userId,
            //            deleted = false,
            //            active = true
            //        },
            //        new tbl_Role()
            //        {
            //            id = Guid.NewGuid(),
            //            name = "Học vụ",
            //            code = "academic",
            //            permissions = "",
            //            created = Timestamp.Now(),
            //            updated = Timestamp.Now(),
            //            createdBy = userId,
            //            updatedBy = userId,
            //            deleted = false,
            //            active = true
            //        },
            //        new tbl_Role()
            //        {
            //            id = Guid.NewGuid(),
            //            name = "Kế toán",
            //            code = "accountant",
            //            permissions = "",
            //            created = Timestamp.Now(),
            //            updated = Timestamp.Now(),
            //            createdBy = userId,
            //            updatedBy = userId,
            //            deleted = false,
            //            active = true
            //        },
            //        new tbl_Role()
            //        {
            //            id = Guid.NewGuid(),
            //            name = "Giáo viên",
            //            code = "teacher",
            //            permissions = "",
            //            created = Timestamp.Now(),
            //            updated = Timestamp.Now(),
            //            createdBy = userId,
            //            updatedBy = userId,
            //            deleted = false,
            //            active = true
            //        },
            //        new tbl_Role()
            //        {
            //            id = Guid.NewGuid(),
            //            name = "Học viên",
            //            code = "student",
            //            permissions = "",
            //            created = Timestamp.Now(),
            //            updated = Timestamp.Now(),
            //            createdBy = userId,
            //            updatedBy = userId,
            //            deleted = false,
            //            active = true
            //        },
            //        new tbl_Role()
            //        {
            //            id = Guid.NewGuid(),
            //            name = "Phụ huynh",
            //            code = "parents",
            //            permissions = "",
            //            created = Timestamp.Now(),
            //            updated = Timestamp.Now(),
            //            createdBy = userId,
            //            updatedBy = userId,
            //            deleted = false,
            //            active = true
            //        }
            //    );
        }
    }
}
