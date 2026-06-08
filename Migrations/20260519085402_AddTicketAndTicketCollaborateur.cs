using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modulehelpdesktimesheet.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketAndTicketCollaborateur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketCollaborateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    SyncedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketCollaborateurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    InterventionId = table.Column<int>(type: "int", nullable: true),
                    Categorie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SousClientId = table.Column<int>(type: "int", nullable: true),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priorite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFermeture = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DureeReelleMinutes = table.Column<double>(type: "float", nullable: false),
                    CoutFinal = table.Column<double>(type: "float", nullable: false),
                    AgentPrincipalId = table.Column<int>(type: "int", nullable: false),
                    CodeUnidesk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SyncedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketCollaborateurs");

            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
