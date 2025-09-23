using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Data.Migrations
{
    /// <inheritdoc />
    public partial class registerDbInitialOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegisterDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Db_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterDb", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisterDb");
        }
    }
}
