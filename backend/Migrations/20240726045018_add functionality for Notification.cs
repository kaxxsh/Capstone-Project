using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class addfunctionalityforNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromUserId",
                table: "Notifies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notifies_FromUserId",
                table: "Notifies",
                column: "FromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifies_AspNetUsers_FromUserId",
                table: "Notifies",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifies_AspNetUsers_FromUserId",
                table: "Notifies");

            migrationBuilder.DropIndex(
                name: "IX_Notifies_FromUserId",
                table: "Notifies");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "Notifies");
        }
    }
}
