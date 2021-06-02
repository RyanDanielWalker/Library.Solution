using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class StringUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CopyPatron_AspNetUsers_PatronId1",
                table: "CopyPatron");

            migrationBuilder.DropIndex(
                name: "IX_CopyPatron_PatronId1",
                table: "CopyPatron");

            migrationBuilder.DropColumn(
                name: "PatronId1",
                table: "CopyPatron");

            migrationBuilder.AlterColumn<string>(
                name: "PatronId",
                table: "CopyPatron",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CopyPatron_PatronId",
                table: "CopyPatron",
                column: "PatronId");

            migrationBuilder.AddForeignKey(
                name: "FK_CopyPatron_AspNetUsers_PatronId",
                table: "CopyPatron",
                column: "PatronId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CopyPatron_AspNetUsers_PatronId",
                table: "CopyPatron");

            migrationBuilder.DropIndex(
                name: "IX_CopyPatron_PatronId",
                table: "CopyPatron");

            migrationBuilder.AlterColumn<int>(
                name: "PatronId",
                table: "CopyPatron",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatronId1",
                table: "CopyPatron",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CopyPatron_PatronId1",
                table: "CopyPatron",
                column: "PatronId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CopyPatron_AspNetUsers_PatronId1",
                table: "CopyPatron",
                column: "PatronId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
