using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordleCloneWars.Data.Migrations
{
    public partial class Unique_Round : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rounds_Type_UserId_GameRound",
                table: "Rounds",
                columns: new[] { "Type", "UserId", "GameRound" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rounds_Type_UserId_GameRound",
                table: "Rounds");
        }
    }
}
