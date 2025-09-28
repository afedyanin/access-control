using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControl.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_feature_key_role_access_roles_RolesId",
                table: "feature_key_role");

            migrationBuilder.DropForeignKey(
                name: "FK_feature_key_role_feature_key_FeatureKeyId",
                table: "feature_key_role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_feature_key_role",
                table: "feature_key_role");

            migrationBuilder.DropIndex(
                name: "IX_feature_key_role_RolesId",
                table: "feature_key_role");

            migrationBuilder.DropPrimaryKey(
                name: "feature_key_pkey",
                table: "feature_key");

            migrationBuilder.DropColumn(
                name: "RolesId",
                table: "feature_key_role");

            migrationBuilder.RenameTable(
                name: "feature_key",
                newName: "feature_keys");

            migrationBuilder.RenameColumn(
                name: "Permissions",
                table: "feature_key_role",
                newName: "permissions");

            migrationBuilder.RenameColumn(
                name: "FeatureKeyId",
                table: "feature_key_role",
                newName: "feature_key_id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "feature_key_role",
                newName: "access_role_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_feature_key_role",
                table: "feature_key_role",
                columns: new[] { "access_role_id", "feature_key_id" });

            migrationBuilder.AddPrimaryKey(
                name: "feature_keys_pkey",
                table: "feature_keys",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_feature_key_role_feature_key_id",
                table: "feature_key_role",
                column: "feature_key_id");

            migrationBuilder.AddForeignKey(
                name: "FK_feature_key_role_access_roles_access_role_id",
                table: "feature_key_role",
                column: "access_role_id",
                principalTable: "access_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_feature_key_role_feature_keys_feature_key_id",
                table: "feature_key_role",
                column: "feature_key_id",
                principalTable: "feature_keys",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_feature_key_role_access_roles_access_role_id",
                table: "feature_key_role");

            migrationBuilder.DropForeignKey(
                name: "FK_feature_key_role_feature_keys_feature_key_id",
                table: "feature_key_role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_feature_key_role",
                table: "feature_key_role");

            migrationBuilder.DropIndex(
                name: "IX_feature_key_role_feature_key_id",
                table: "feature_key_role");

            migrationBuilder.DropPrimaryKey(
                name: "feature_keys_pkey",
                table: "feature_keys");

            migrationBuilder.RenameTable(
                name: "feature_keys",
                newName: "feature_key");

            migrationBuilder.RenameColumn(
                name: "permissions",
                table: "feature_key_role",
                newName: "Permissions");

            migrationBuilder.RenameColumn(
                name: "feature_key_id",
                table: "feature_key_role",
                newName: "FeatureKeyId");

            migrationBuilder.RenameColumn(
                name: "access_role_id",
                table: "feature_key_role",
                newName: "RoleId");

            migrationBuilder.AddColumn<Guid>(
                name: "RolesId",
                table: "feature_key_role",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_feature_key_role",
                table: "feature_key_role",
                columns: new[] { "FeatureKeyId", "RolesId" });

            migrationBuilder.AddPrimaryKey(
                name: "feature_key_pkey",
                table: "feature_key",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_feature_key_role_RolesId",
                table: "feature_key_role",
                column: "RolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_feature_key_role_access_roles_RolesId",
                table: "feature_key_role",
                column: "RolesId",
                principalTable: "access_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_feature_key_role_feature_key_FeatureKeyId",
                table: "feature_key_role",
                column: "FeatureKeyId",
                principalTable: "feature_key",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
