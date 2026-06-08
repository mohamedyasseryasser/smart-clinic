using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smart_clinic.Migrations
{
    /// <inheritdoc />
    public partial class sdidoee5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_AspNetUsers_AddedBy",
                table: "Medicines");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_AddedBy",
                table: "Medicines");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Medicines",
                newName: "CategoryName");

            migrationBuilder.AlterColumn<string>(
                name: "AddedBy",
                table: "Medicines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Medicines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "Medicines",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_user_id",
                table: "Medicines",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_AspNetUsers_user_id",
                table: "Medicines",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_AspNetUsers_user_id",
                table: "Medicines");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_user_id",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "Medicines");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Medicines",
                newName: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "AddedBy",
                table: "Medicines",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_AddedBy",
                table: "Medicines",
                column: "AddedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_AspNetUsers_AddedBy",
                table: "Medicines",
                column: "AddedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
