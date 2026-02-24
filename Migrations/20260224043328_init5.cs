using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student_Project_Assignment.Migrations
{
    /// <inheritdoc />
    public partial class init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$IU0VY8BeQWlZTdnrNkaf5e.nv/Jo.dLlrPIGbDv/v5BY4lvC7kzxy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$R4Gt4QNJJNht6TcYu8bd5uoJ0nESzdwFZp51oRcnbMKBlBE0ZNeES");
        }
    }
}
