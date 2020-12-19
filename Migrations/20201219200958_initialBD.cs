using Microsoft.EntityFrameworkCore.Migrations;

namespace EmpresaIT.Migrations
{
    public partial class initialBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Contraseña = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreCompleto = table.Column<string>(nullable: false),
                    Contraseña = table.Column<string>(nullable: false),
                    Edad = table.Column<int>(nullable: false),
                    Sexo = table.Column<string>(nullable: false),
                    PuestoDeTrabajo = table.Column<string>(nullable: false),
                    Sueldo = table.Column<int>(nullable: false),
                    EmpresaEmail = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Email);
                    table.ForeignKey(
                        name: "FK_Empleados_Empresas_EmpresaEmail",
                        column: x => x.EmpresaEmail,
                        principalTable: "Empresas",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_EmpresaEmail",
                table: "Empleados",
                column: "EmpresaEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
