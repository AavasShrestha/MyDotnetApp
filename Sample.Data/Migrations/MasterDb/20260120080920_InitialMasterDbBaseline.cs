using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Data.Migrations.MasterDb
{
    public partial class InitialMasterDbBaseline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // intentionally empty — Tenants table already exists
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // intentionally empty
        }
    }
}
