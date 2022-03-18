using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordleCloneWars.Data.Migrations;

public partial class Round_AddCompletionTime : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<long>(
            name: "CompletedDateTime",
            table: "Rounds",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0L);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CompletedDateTime",
            table: "Rounds");
    }
}