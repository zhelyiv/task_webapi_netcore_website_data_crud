using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Ef.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebsiteCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteHomepageSnapshot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    DateUpdatedUtc = table.Column<DateTime>(nullable: true),
                    Image = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteHomepageSnapshot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Website",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    DateUpdatedUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    HomepageSnapshotId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Website_WebsiteCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "WebsiteCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Website_WebsiteHomepageSnapshot_HomepageSnapshotId",
                        column: x => x.HomepageSnapshotId,
                        principalTable: "WebsiteHomepageSnapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Website_WebsiteStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WebsiteStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteField",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    DateUpdatedUtc = table.Column<DateTime>(nullable: true),
                    WebsiteId = table.Column<int>(nullable: false),
                    FieldName = table.Column<string>(nullable: false),
                    FieldValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsiteField_Website_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Website",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteLogin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    DateUpdatedUtc = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    WebsiteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsiteLogin_Website_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Website",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "WebsiteCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "None" },
                    { 1, "Commercial" },
                    { 2, "Business" },
                    { 3, "Blog" },
                    { 4, "Retail" },
                    { 5, "Rcommerce" }
                });

            migrationBuilder.InsertData(
                table: "WebsiteStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Active" },
                    { 1, "Inactive" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Website_CategoryId",
                table: "Website",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Website_HomepageSnapshotId",
                table: "Website",
                column: "HomepageSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Website_Name",
                table: "Website",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Website_StatusId",
                table: "Website",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Website_Url",
                table: "Website",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteCategory_Name",
                table: "WebsiteCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteField_WebsiteId_FieldName",
                table: "WebsiteField",
                columns: new[] { "WebsiteId", "FieldName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteLogin_WebsiteId_Email",
                table: "WebsiteLogin",
                columns: new[] { "WebsiteId", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteStatus_Name",
                table: "WebsiteStatus",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebsiteField");

            migrationBuilder.DropTable(
                name: "WebsiteLogin");

            migrationBuilder.DropTable(
                name: "Website");

            migrationBuilder.DropTable(
                name: "WebsiteCategory");

            migrationBuilder.DropTable(
                name: "WebsiteHomepageSnapshot");

            migrationBuilder.DropTable(
                name: "WebsiteStatus");
        }
    }
}
