using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zuma.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ToDoItemToTelegramFriendly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                table: "ToDoItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "ToDoItems");
        }
    }
}
