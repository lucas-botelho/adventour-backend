using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adventour.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationStepToPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "registration_step",
                table: "Person",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "registration_step",
                table: "Person");
        }
    }
}
