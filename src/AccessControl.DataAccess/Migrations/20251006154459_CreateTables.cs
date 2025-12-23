using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControl.DataAccess.Migrations;

/// <inheritdoc />
public partial class CreateTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "access_control");

        migrationBuilder.CreateTable(
            name: "feature_keys",
            schema: "access_control",
            columns: table => new
            {
                name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("feature_keys_pkey", x => x.name);
            });

        migrationBuilder.CreateTable(
            name: "resources",
            schema: "access_control",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("resources_pkey", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "roles",
            schema: "access_control",
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
            name: "users",
            schema: "access_control",
            columns: table => new
            {
                name = table.Column<string>(type: "text", nullable: false),
                email = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("users_pkey", x => x.name);
            });

        migrationBuilder.CreateTable(
            name: "feature_key_role",
            schema: "access_control",
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
                    principalSchema: "access_control",
                    principalTable: "feature_keys",
                    principalColumn: "name",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_feature_key_role_roles_role_name",
                    column: x => x.role_name,
                    principalSchema: "access_control",
                    principalTable: "roles",
                    principalColumn: "name",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "resource_role",
            schema: "access_control",
            columns: table => new
            {
                resource_id = table.Column<Guid>(type: "uuid", nullable: false),
                role_name = table.Column<string>(type: "text", nullable: false),
                permissions = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_resource_role", x => new { x.resource_id, x.role_name });
                table.ForeignKey(
                    name: "FK_resource_role_resources_resource_id",
                    column: x => x.resource_id,
                    principalSchema: "access_control",
                    principalTable: "resources",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_resource_role_roles_role_name",
                    column: x => x.role_name,
                    principalSchema: "access_control",
                    principalTable: "roles",
                    principalColumn: "name",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "user_role",
            schema: "access_control",
            columns: table => new
            {
                user_name = table.Column<string>(type: "text", nullable: false),
                role_name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_user_role", x => new { x.role_name, x.user_name });
                table.ForeignKey(
                    name: "FK_user_role_roles_role_name",
                    column: x => x.role_name,
                    principalSchema: "access_control",
                    principalTable: "roles",
                    principalColumn: "name",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_user_role_users_user_name",
                    column: x => x.user_name,
                    principalSchema: "access_control",
                    principalTable: "users",
                    principalColumn: "name",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_feature_key_role_role_name",
            schema: "access_control",
            table: "feature_key_role",
            column: "role_name");

        migrationBuilder.CreateIndex(
            name: "IX_resource_role_role_name",
            schema: "access_control",
            table: "resource_role",
            column: "role_name");

        migrationBuilder.CreateIndex(
            name: "IX_user_role_user_name",
            schema: "access_control",
            table: "user_role",
            column: "user_name");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "feature_key_role",
            schema: "access_control");

        migrationBuilder.DropTable(
            name: "resource_role",
            schema: "access_control");

        migrationBuilder.DropTable(
            name: "user_role",
            schema: "access_control");

        migrationBuilder.DropTable(
            name: "feature_keys",
            schema: "access_control");

        migrationBuilder.DropTable(
            name: "resources",
            schema: "access_control");

        migrationBuilder.DropTable(
            name: "roles",
            schema: "access_control");

        migrationBuilder.DropTable(
            name: "users",
            schema: "access_control");
    }
}
