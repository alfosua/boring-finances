using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BoringSoftware.Finances.Data.Migrations
{
    public partial class Initial_Operations_Accounts_FinancialUnits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialUnitTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialUnitTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationEntryTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationEntryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Kebab = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    AccountTypeId = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FinancialUnitTypeId = table.Column<byte>(type: "smallint", nullable: false),
                    Kebab = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialUnits_FinancialUnitTypes_FinancialUnitTypeId",
                        column: x => x.FinancialUnitTypeId,
                        principalTable: "FinancialUnitTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationNotes_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    FinancialUnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationEntryTypeId = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationEntries_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationEntries_FinancialUnits_FinancialUnitId",
                        column: x => x.FinancialUnitId,
                        principalTable: "FinancialUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationEntries_OperationEntryTypes_OperationEntryTypeId",
                        column: x => x.OperationEntryTypeId,
                        principalTable: "OperationEntryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationEntries_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationEntryNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationEntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationEntryNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationEntryNotes_OperationEntries_OperationEntryId",
                        column: x => x.OperationEntryId,
                        principalTable: "OperationEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccountTypes",
                columns: new[] { "Id", "Code" },
                values: new object[,]
                {
                    { (byte)1, "Equity" },
                    { (byte)2, "Good" },
                    { (byte)3, "Liability" },
                    { (byte)4, "Income" },
                    { (byte)5, "Expense" },
                    { (byte)6, "Trading" }
                });

            migrationBuilder.InsertData(
                table: "FinancialUnitTypes",
                columns: new[] { "Id", "Code" },
                values: new object[,]
                {
                    { (byte)1, "Currency" },
                    { (byte)255, "Other" }
                });

            migrationBuilder.InsertData(
                table: "OperationEntryTypes",
                columns: new[] { "Id", "Code" },
                values: new object[,]
                {
                    { (byte)1, "Debit" },
                    { (byte)2, "Credit" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTypeId",
                table: "Accounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialUnits_FinancialUnitTypeId",
                table: "FinancialUnits",
                column: "FinancialUnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationEntries_AccountId",
                table: "OperationEntries",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationEntries_FinancialUnitId",
                table: "OperationEntries",
                column: "FinancialUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationEntries_OperationEntryTypeId",
                table: "OperationEntries",
                column: "OperationEntryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationEntries_OperationId",
                table: "OperationEntries",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationEntryNotes_OperationEntryId",
                table: "OperationEntryNotes",
                column: "OperationEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationNotes_OperationId",
                table: "OperationNotes",
                column: "OperationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationEntryNotes");

            migrationBuilder.DropTable(
                name: "OperationNotes");

            migrationBuilder.DropTable(
                name: "OperationEntries");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "FinancialUnits");

            migrationBuilder.DropTable(
                name: "OperationEntryTypes");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "AccountTypes");

            migrationBuilder.DropTable(
                name: "FinancialUnitTypes");
        }
    }
}
