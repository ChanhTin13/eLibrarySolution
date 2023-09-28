using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Authors",
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
                    table.PrimaryKey("PK_tbl_Authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Books",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    counterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    titleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Books", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Titles",
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
                    created = table.Column<double>(type: "float", nullable: true),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updated = table.Column<double>(type: "float", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Titles", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Authors");

            migrationBuilder.DropTable(
                name: "tbl_Books");

            migrationBuilder.DropTable(
                name: "tbl_Titles");
        }
    }
}
