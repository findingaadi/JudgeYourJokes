using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JudgeYourJokes.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToJokes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Jokes");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Jokes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jokes_UserID",
                table: "Jokes",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jokes_AspNetUsers_UserID",
                table: "Jokes",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jokes_AspNetUsers_UserID",
                table: "Jokes");

            migrationBuilder.DropIndex(
                name: "IX_Jokes_UserID",
                table: "Jokes");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Jokes");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Jokes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
