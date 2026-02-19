using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JournalChallenge.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "ExceptionJournal",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QueryParams = table.Column<string>(type: "jsonb", nullable: false),
                    BodyParams = table.Column<string>(type: "jsonb", nullable: false),
                    StackTrace = table.Column<string>(type: "text", nullable: false),
                    ExceptionType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionJournal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trees",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    TreeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nodes_Nodes_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nodes_Trees_TreeId",
                        column: x => x.TreeId,
                        principalSchema: "public",
                        principalTable: "Trees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_ParentId",
                schema: "public",
                table: "Nodes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_TreeId_ParentId_Name",
                schema: "public",
                table: "Nodes",
                columns: new[] { "TreeId", "ParentId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trees_Name",
                schema: "public",
                table: "Trees",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionJournal",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Nodes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Trees",
                schema: "public");
        }
    }
}
