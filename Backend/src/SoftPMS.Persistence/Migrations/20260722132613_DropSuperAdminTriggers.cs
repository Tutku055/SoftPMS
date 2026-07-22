using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftPMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropSuperAdminTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS tr_ProtectSuperAdminRole;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS tr_ProtectSuperAdminUser;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Triggers are now handled in the Application layer (C#), so no need to recreate them on rollback.
        }
    }
}
