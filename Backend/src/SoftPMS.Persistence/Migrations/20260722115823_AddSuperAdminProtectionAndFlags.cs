using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftPMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSuperAdminProtectionAndFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemUser",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresPasswordChange",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"
                CREATE TRIGGER tr_ProtectSuperAdminRole
                ON Roles
                INSTEAD OF DELETE
                AS
                BEGIN
                    IF EXISTS (SELECT 1 FROM deleted WHERE Name = 'Super Admin')
                    BEGIN
                        RAISERROR('Cannot delete the Super Admin role.', 16, 1);
                        ROLLBACK TRANSACTION;
                        RETURN;
                    END
                    DELETE FROM Roles WHERE Id IN (SELECT Id FROM deleted);
                END
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER tr_ProtectSuperAdminUser
                ON Users
                INSTEAD OF DELETE
                AS
                BEGIN
                    IF EXISTS (SELECT 1 FROM deleted WHERE Username = 'SuperAdmin')
                    BEGIN
                        RAISERROR('Cannot delete the Super Admin user.', 16, 1);
                        ROLLBACK TRANSACTION;
                        RETURN;
                    END
                    DELETE FROM Users WHERE Id IN (SELECT Id FROM deleted);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemUser",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RequiresPasswordChange",
                table: "Users");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS tr_ProtectSuperAdminRole;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS tr_ProtectSuperAdminUser;");
        }
    }
}
