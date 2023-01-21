using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestApplication.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "87a1532f-21b5-4454-a7dc-c9fd816c18ed",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "385f7596-a82f-4520-b99b-040e4aa5f9dc", "d2c3b230-56f7-45f9-b954-d118bf6bdf4f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "87a1532f-21b5-4454-a7dc-c9fd816c18ed",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e3e0b1dd-332c-42f4-85bb-e3a3462b24a0", "d8ad0604-f2c0-4785-b3d1-15899f637289" });
        }
    }
}
