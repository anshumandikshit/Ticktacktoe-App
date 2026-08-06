using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class ScoreBoardTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Player",
                table: "Scoreboards");

            migrationBuilder.RenameColumn(
                name: "Wins",
                table: "Scoreboards",
                newName: "XWins");

            migrationBuilder.RenameColumn(
                name: "Losses",
                table: "Scoreboards",
                newName: "OWins");

            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "Scoreboards",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Scoreboards_SessionId",
                table: "Scoreboards",
                column: "SessionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Scoreboards_SessionId",
                table: "Scoreboards");

            migrationBuilder.RenameColumn(
                name: "XWins",
                table: "Scoreboards",
                newName: "Wins");

            migrationBuilder.RenameColumn(
                name: "OWins",
                table: "Scoreboards",
                newName: "Losses");

            migrationBuilder.AlterColumn<Guid>(
                name: "SessionId",
                table: "Scoreboards",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Player",
                table: "Scoreboards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
