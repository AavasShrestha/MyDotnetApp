using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Data.Migrations
{
    /// <inheritdoc />
    public partial class clientDetailsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "tblClientDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ApprovalSystem",
                table: "tblClientDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CollectionApp",
                table: "tblClientDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "tblClientDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Primary_email",
                table: "tblClientDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Primary_phone",
                table: "tblClientDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SMS_service",
                table: "tblClientDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Secondary_email",
                table: "tblClientDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Secondary_phone",
                table: "tblClientDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "tblClientDetails");

            migrationBuilder.DropColumn(
                name: "ApprovalSystem",
                table: "tblClientDetails");

            migrationBuilder.DropColumn(
                name: "CollectionApp",
                table: "tblClientDetails");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "tblClientDetails");

            migrationBuilder.DropColumn(
                name: "Primary_email",
                table: "tblClientDetails");

            migrationBuilder.DropColumn(
                name: "Primary_phone",
                table: "tblClientDetails");

            migrationBuilder.DropColumn(
                name: "SMS_service",
                table: "tblClientDetails");

            migrationBuilder.DropColumn(
                name: "Secondary_email",
                table: "tblClientDetails");

            migrationBuilder.DropColumn(
                name: "Secondary_phone",
                table: "tblClientDetails");
        }
    }
}
