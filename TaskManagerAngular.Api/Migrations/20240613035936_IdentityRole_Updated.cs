using Microsoft.EntityFrameworkCore.Migrations;
//using System.Data.Entity.Migration;
#nullable disable

namespace TaskManagerAngular.Api.Migrations
{
    /// <inheritdoc />
    public partial class IdentityRole_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //DropColumn("dbo.AspNetUsers", "Discriminator");
        }
    }
}
