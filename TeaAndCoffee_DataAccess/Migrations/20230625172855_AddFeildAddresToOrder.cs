using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeaAndCoffe_DataAcces.Migrations
{
    /// <inheritdoc />
    public partial class AddFeildAddresToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "State",
                table: "OrderHeader");

            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "OrderHeader",
                newName: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "OrderHeader",
                newName: "StreetAddress");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
