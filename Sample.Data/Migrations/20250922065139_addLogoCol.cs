using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Data.Migrations
{
    /// <inheritdoc />
    public partial class addLogoCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "tblClientDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "tblClientDetails");
        }
    }
}
