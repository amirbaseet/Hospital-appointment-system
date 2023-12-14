using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_appointment_system.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_DepartmentsDepartmentID",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_DepartmentsDepartmentID",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DepartmentsDepartmentID",
                table: "Doctors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentsDepartmentID",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DepartmentsDepartmentID",
                table: "Doctors",
                column: "DepartmentsDepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_DepartmentsDepartmentID",
                table: "Doctors",
                column: "DepartmentsDepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID");
        }
    }
}
