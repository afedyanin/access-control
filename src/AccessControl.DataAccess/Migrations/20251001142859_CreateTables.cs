using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControl.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "feature_keys",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("feature_keys_pkey", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("roles_pkey", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "feature_key_role",
                columns: table => new
                {
                    feature_key_name = table.Column<string>(type: "text", nullable: false),
                    role_name = table.Column<string>(type: "text", nullable: false),
                    permissions = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feature_key_role", x => new { x.feature_key_name, x.role_name });
                    table.ForeignKey(
                        name: "FK_feature_key_role_feature_keys_feature_key_name",
                        column: x => x.feature_key_name,
                        principalTable: "feature_keys",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_feature_key_role_roles_role_name",
                        column: x => x.role_name,
                        principalTable: "roles",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_feature_key_role_role_name",
                table: "feature_key_role",
                column: "role_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feature_key_role");

            migrationBuilder.DropTable(
                name: "feature_keys");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
