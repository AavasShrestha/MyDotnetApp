using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblClientDetails",
                columns: table => new
                {
                    client_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    client_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    db_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_by = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblClientDetails", x => x.client_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblClientDetails");
        }
    }
}
