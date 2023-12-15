using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_appointment_system.Migrations
{
    public partial class Secound : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_DepartmentID",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "DepartmentID",
                table: "Doctors",
                newName: "ClinicID");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_DepartmentID",
                table: "Doctors",
                newName: "IX_Doctors_ClinicID");

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
                name: "FK_Doctors_Clinic_ClinicID",
                table: "Doctors",
                column: "ClinicID",
                principalTable: "Clinic",
                principalColumn: "ClinicID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_DepartmentsDepartmentID",
                table: "Doctors",
                column: "DepartmentsDepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Clinic_ClinicID",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_DepartmentsDepartmentID",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_DepartmentsDepartmentID",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DepartmentsDepartmentID",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "ClinicID",
                table: "Doctors",
                newName: "DepartmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_ClinicID",
                table: "Doctors",
                newName: "IX_Doctors_DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_DepartmentID",
                table: "Doctors",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
