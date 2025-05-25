using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var rootDirectory = Path.GetFullPath(Path.Combine(currentDirectory, ".."));

            var photoApprovalStatsPath = Path.Combine(rootDirectory, "DatingApp.Infrastructure", "Data", "Procedures", "GetPhotoApprovalStats.sql");
            var usersWithoutMainPhotoPath = Path.Combine(rootDirectory, "DatingApp.Infrastructure", "Data", "Procedures", "GetUsersWithoutMainPhoto.sql");

            migrationBuilder.Sql(File.ReadAllText(photoApprovalStatsPath));
            migrationBuilder.Sql(File.ReadAllText(usersWithoutMainPhotoPath));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetPhotoApprovalStats;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetUsersWithoutMainPhoto;");
        }
    }
}
