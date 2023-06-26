using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeaAndCoffe_DataAcces.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderHeaderAddStatusOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderHeader");
        }
    }
}
