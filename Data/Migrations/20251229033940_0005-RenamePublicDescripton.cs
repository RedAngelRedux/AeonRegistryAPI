using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AeonRegistryAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class _0005RenamePublicDescripton : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicDescription",
                table: "Artifacts",
                newName: "PublicNarrative");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicNarrative",
                table: "Artifacts",
                newName: "PublicDescription");
        }
    }
}
