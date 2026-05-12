using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smart_clinic.Migrations
{
    /// <inheritdoc />
    public partial class insssldo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "Medicines",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "cat_id",
                table: "Medicines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    cat_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cat_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cat_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.cat_id);
                    table.ForeignKey(
                        name: "FK_categories_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_AddedBy",
                table: "Medicines",
                column: "AddedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_cat_id",
                table: "Medicines",
                column: "cat_id");

            migrationBuilder.CreateIndex(
                name: "IX_categories_user_id",
                table: "categories",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_AspNetUsers_AddedBy",
                table: "Medicines",
                column: "AddedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_categories_cat_id",
                table: "Medicines",
                column: "cat_id",
                principalTable: "categories",
                principalColumn: "cat_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_AspNetUsers_AddedBy",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_categories_cat_id",
                table: "Medicines");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_AddedBy",
                table: "Medicines");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_cat_id",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "cat_id",
                table: "Medicines");
        }
    }
}
