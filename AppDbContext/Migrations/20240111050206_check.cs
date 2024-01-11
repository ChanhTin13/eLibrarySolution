using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class check : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Author",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Author", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Book",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    titleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isLost = table.Column<bool>(type: "bit", nullable: false),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Book", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Cities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Cities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Counter",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    counterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Counter", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Districts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Districts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_LibraryLoan",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    readerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    librarianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    totalBook = table.Column<int>(type: "int", nullable: false),
                    listBookLoan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isReturnAll = table.Column<bool>(type: "bit", nullable: false),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_LibraryLoan", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Necessary",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type = table.Column<int>(type: "int", nullable: true),
                    typeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Necessary", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    permissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Title",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    quantityReal = table.Column<int>(type: "int", nullable: false),
                    quantityLoaning = table.Column<int>(type: "int", nullable: false),
                    quantityLost = table.Column<int>(type: "int", nullable: false),
                    thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    authorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    categoryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Title", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    birthday = table.Column<double>(type: "float", nullable: true),
                    password = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    gender = table.Column<int>(type: "int", nullable: true),
                    isAdmin = table.Column<bool>(type: "bit", nullable: true),
                    roles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    districtId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    cityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    wardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    keyForgotPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdDateKeyForgot = table.Column<double>(type: "float", nullable: true),
                    oneSignalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Wards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    districtId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Wards", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Author");

            migrationBuilder.DropTable(
                name: "tbl_Book");

            migrationBuilder.DropTable(
                name: "tbl_Category");

            migrationBuilder.DropTable(
                name: "tbl_Cities");

            migrationBuilder.DropTable(
                name: "tbl_Counter");

            migrationBuilder.DropTable(
                name: "tbl_Districts");

            migrationBuilder.DropTable(
                name: "tbl_LibraryLoan");

            migrationBuilder.DropTable(
                name: "tbl_Necessary");

            migrationBuilder.DropTable(
                name: "tbl_Role");

            migrationBuilder.DropTable(
                name: "tbl_Title");

            migrationBuilder.DropTable(
                name: "tbl_Users");

            migrationBuilder.DropTable(
                name: "tbl_Wards");
        }
    }
}
