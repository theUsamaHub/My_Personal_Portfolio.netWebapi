using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Personal_Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class FixGalleryImagesType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GalleryImages",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GalleryImages",
                table: "Profiles",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
