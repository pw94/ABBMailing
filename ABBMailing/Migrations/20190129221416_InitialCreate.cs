using Microsoft.EntityFrameworkCore.Migrations;

namespace ABBMailing.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(nullable: false),
                    UnsubscribeToken = table.Column<string>(nullable: false),
                    Subscribed = table.Column<bool>(nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressTopic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressId = table.Column<int>(nullable: false),
                    TopicId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressTopic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressTopic_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddressTopic_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Email",
                table: "Addresses",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UnsubscribeToken",
                table: "Addresses",
                column: "UnsubscribeToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressTopic_AddressId",
                table: "AddressTopic",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressTopic_TopicId",
                table: "AddressTopic",
                column: "TopicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressTopic");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Topics");
        }
    }
}
