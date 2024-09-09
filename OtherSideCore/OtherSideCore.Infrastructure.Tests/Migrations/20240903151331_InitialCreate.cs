using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtherSideCore.Infrastructure.Tests.Migrations
{
   /// <inheritdoc />
   public partial class InitialCreate : Migration
   {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.CreateTable(
             name: "Users",
             columns: table => new
             {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                     .Annotation("Sqlite:Autoincrement", true),
                CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<int>(type: "INTEGER", nullable: false),
                LastModifiedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                LastModifiedById = table.Column<int>(type: "INTEGER", nullable: false),
                IsSuperAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                FirstName = table.Column<string>(type: "TEXT", nullable: true),
                LastName = table.Column<string>(type: "TEXT", nullable: true),
                UserName = table.Column<string>(type: "TEXT", nullable: true)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Users", x => x.Id);
                table.ForeignKey(
                       name: "FK_Users_Users_CreatedById",
                       column: x => x.CreatedById,
                       principalTable: "Users",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                       name: "FK_Users_Users_LastModifiedById",
                       column: x => x.LastModifiedById,
                       principalTable: "Users",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Restrict);
             });

         migrationBuilder.CreateTable(
                name: "TestEntities",
                columns: table => new
                {
                   Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                   CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                   CreatedById = table.Column<int>(type: "INTEGER", nullable: false),
                   LastModifiedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                   LastModifiedById = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                   table.PrimaryKey("PK_TestEntities", x => x.Id);
                   table.ForeignKey(
                       name: "FK_TestEntities_Users_CreatedById",
                       column: x => x.CreatedById,
                       principalTable: "Users",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
                   table.ForeignKey(
                       name: "FK_TestEntities_Users_LastModifiedById",
                       column: x => x.LastModifiedById,
                       principalTable: "Users",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
                });

         migrationBuilder.CreateIndex(
             name: "IX_TestEntities_CreatedById",
             table: "TestEntities",
             column: "CreatedById");

         migrationBuilder.CreateIndex(
             name: "IX_TestEntities_LastModifiedById",
             table: "TestEntities",
             column: "LastModifiedById");

         migrationBuilder.CreateIndex(
             name: "IX_Users_CreatedById",
             table: "Users",
             column: "CreatedById");

         migrationBuilder.CreateIndex(
             name: "IX_Users_LastModifiedById",
             table: "Users",
             column: "LastModifiedById");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropTable(
             name: "TestEntities");

         migrationBuilder.DropTable(
             name: "Users");
      }
   }
}
