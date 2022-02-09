using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BoringFinances.Operations.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    AccountTypeId = table.Column<byte>(type: "smallint", nullable: false),
                    Code = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.AccountTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FinancialUnitTypes",
                columns: table => new
                {
                    FinancialUnitTypeId = table.Column<byte>(type: "smallint", nullable: false),
                    Code = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialUnitTypes", x => x.FinancialUnitTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    NoteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                });

            migrationBuilder.CreateTable(
                name: "OperationActions",
                columns: table => new
                {
                    OperationActionId = table.Column<byte>(type: "smallint", nullable: false),
                    Code = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationActions", x => x.OperationActionId);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    OperationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.OperationId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    AccountTypeId = table.Column<byte>(type: "smallint", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "AccountTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialUnits",
                columns: table => new
                {
                    FinancialUnitId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FinancialUnitTypeId = table.Column<byte>(type: "smallint", nullable: false),
                    FractionalDigits = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialUnits", x => x.FinancialUnitId);
                    table.ForeignKey(
                        name: "FK_FinancialUnits_FinancialUnitTypes_FinancialUnitTypeId",
                        column: x => x.FinancialUnitTypeId,
                        principalTable: "FinancialUnitTypes",
                        principalColumn: "FinancialUnitTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteOperation",
                columns: table => new
                {
                    NotesNoteId = table.Column<long>(type: "bigint", nullable: false),
                    OperationsOperationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteOperation", x => new { x.NotesNoteId, x.OperationsOperationId });
                    table.ForeignKey(
                        name: "FK_NoteOperation_Notes_NotesNoteId",
                        column: x => x.NotesNoteId,
                        principalTable: "Notes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteOperation_Operations_OperationsOperationId",
                        column: x => x.OperationsOperationId,
                        principalTable: "Operations",
                        principalColumn: "OperationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationEntries",
                columns: table => new
                {
                    OperationId = table.Column<long>(type: "bigint", nullable: false),
                    OperationEntryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationEntries", x => new { x.OperationId, x.OperationEntryId });
                    table.ForeignKey(
                        name: "FK_OperationEntries_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "OperationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationTag",
                columns: table => new
                {
                    OperationsOperationId = table.Column<long>(type: "bigint", nullable: false),
                    TagsTagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTag", x => new { x.OperationsOperationId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_OperationTag_Operations_OperationsOperationId",
                        column: x => x.OperationsOperationId,
                        principalTable: "Operations",
                        principalColumn: "OperationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationTag_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountNote",
                columns: table => new
                {
                    AccountsAccountId = table.Column<long>(type: "bigint", nullable: false),
                    NotesNoteId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountNote", x => new { x.AccountsAccountId, x.NotesNoteId });
                    table.ForeignKey(
                        name: "FK_AccountNote_Accounts_AccountsAccountId",
                        column: x => x.AccountsAccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountNote_Notes_NotesNoteId",
                        column: x => x.NotesNoteId,
                        principalTable: "Notes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountTag",
                columns: table => new
                {
                    AccountsAccountId = table.Column<long>(type: "bigint", nullable: false),
                    TagsTagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTag", x => new { x.AccountsAccountId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_AccountTag_Accounts_AccountsAccountId",
                        column: x => x.AccountsAccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountTag_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialUnitNote",
                columns: table => new
                {
                    FinancialUnitsFinancialUnitId = table.Column<long>(type: "bigint", nullable: false),
                    NotesNoteId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialUnitNote", x => new { x.FinancialUnitsFinancialUnitId, x.NotesNoteId });
                    table.ForeignKey(
                        name: "FK_FinancialUnitNote_FinancialUnits_FinancialUnitsFinancialUni~",
                        column: x => x.FinancialUnitsFinancialUnitId,
                        principalTable: "FinancialUnits",
                        principalColumn: "FinancialUnitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialUnitNote_Notes_NotesNoteId",
                        column: x => x.NotesNoteId,
                        principalTable: "Notes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialUnitTag",
                columns: table => new
                {
                    FinancialUnitsFinancialUnitId = table.Column<long>(type: "bigint", nullable: false),
                    TagsTagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialUnitTag", x => new { x.FinancialUnitsFinancialUnitId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_FinancialUnitTag_FinancialUnits_FinancialUnitsFinancialUnit~",
                        column: x => x.FinancialUnitsFinancialUnitId,
                        principalTable: "FinancialUnits",
                        principalColumn: "FinancialUnitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialUnitTag_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteOperationEntry",
                columns: table => new
                {
                    NotesNoteId = table.Column<long>(type: "bigint", nullable: false),
                    OperationEntriesOperationId = table.Column<long>(type: "bigint", nullable: false),
                    OperationEntriesOperationEntryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteOperationEntry", x => new { x.NotesNoteId, x.OperationEntriesOperationId, x.OperationEntriesOperationEntryId });
                    table.ForeignKey(
                        name: "FK_NoteOperationEntry_Notes_NotesNoteId",
                        column: x => x.NotesNoteId,
                        principalTable: "Notes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteOperationEntry_OperationEntries_OperationEntriesOperati~",
                        columns: x => new { x.OperationEntriesOperationId, x.OperationEntriesOperationEntryId },
                        principalTable: "OperationEntries",
                        principalColumns: new[] { "OperationId", "OperationEntryId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationEntryVersions",
                columns: table => new
                {
                    OperationId = table.Column<long>(type: "bigint", nullable: false),
                    OperationEntryId = table.Column<int>(type: "integer", nullable: false),
                    Effective = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    FinancialUnitId = table.Column<long>(type: "bigint", nullable: false),
                    OperationActionId = table.Column<byte>(type: "smallint", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationEntryVersions", x => new { x.OperationId, x.OperationEntryId, x.Effective });
                    table.ForeignKey(
                        name: "FK_OperationEntryVersions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationEntryVersions_FinancialUnits_FinancialUnitId",
                        column: x => x.FinancialUnitId,
                        principalTable: "FinancialUnits",
                        principalColumn: "FinancialUnitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationEntryVersions_OperationActions_OperationActionId",
                        column: x => x.OperationActionId,
                        principalTable: "OperationActions",
                        principalColumn: "OperationActionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationEntryVersions_OperationEntries_OperationId_Operati~",
                        columns: x => new { x.OperationId, x.OperationEntryId },
                        principalTable: "OperationEntries",
                        principalColumns: new[] { "OperationId", "OperationEntryId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationEntryVersions_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "OperationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccountTypes",
                columns: new[] { "AccountTypeId", "Code" },
                values: new object[,]
                {
                    { (byte)1, "Equity" },
                    { (byte)2, "Asset" },
                    { (byte)3, "Liability" },
                    { (byte)4, "Income" },
                    { (byte)5, "Expense" },
                    { (byte)6, "Exchange" }
                });

            migrationBuilder.InsertData(
                table: "FinancialUnitTypes",
                columns: new[] { "FinancialUnitTypeId", "Code" },
                values: new object[,]
                {
                    { (byte)1, "Other" },
                    { (byte)2, "FiatCurrency" },
                    { (byte)3, "CryptoCurrency" }
                });

            migrationBuilder.InsertData(
                table: "OperationActions",
                columns: new[] { "OperationActionId", "Code" },
                values: new object[,]
                {
                    { (byte)1, "Debit" },
                    { (byte)2, "Credit" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountNote_NotesNoteId",
                table: "AccountNote",
                column: "NotesNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTypeId",
                table: "Accounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Code",
                table: "Accounts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountTag_TagsTagId",
                table: "AccountTag",
                column: "TagsTagId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialUnitNote_NotesNoteId",
                table: "FinancialUnitNote",
                column: "NotesNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialUnits_Code",
                table: "FinancialUnits",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialUnits_FinancialUnitTypeId",
                table: "FinancialUnits",
                column: "FinancialUnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialUnitTag_TagsTagId",
                table: "FinancialUnitTag",
                column: "TagsTagId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteOperation_OperationsOperationId",
                table: "NoteOperation",
                column: "OperationsOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteOperationEntry_OperationEntriesOperationId_OperationEnt~",
                table: "NoteOperationEntry",
                columns: new[] { "OperationEntriesOperationId", "OperationEntriesOperationEntryId" });

            migrationBuilder.CreateIndex(
                name: "IX_OperationEntryVersions_AccountId",
                table: "OperationEntryVersions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationEntryVersions_FinancialUnitId",
                table: "OperationEntryVersions",
                column: "FinancialUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationEntryVersions_OperationActionId",
                table: "OperationEntryVersions",
                column: "OperationActionId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationTag_TagsTagId",
                table: "OperationTag",
                column: "TagsTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountNote");

            migrationBuilder.DropTable(
                name: "AccountTag");

            migrationBuilder.DropTable(
                name: "FinancialUnitNote");

            migrationBuilder.DropTable(
                name: "FinancialUnitTag");

            migrationBuilder.DropTable(
                name: "NoteOperation");

            migrationBuilder.DropTable(
                name: "NoteOperationEntry");

            migrationBuilder.DropTable(
                name: "OperationEntryVersions");

            migrationBuilder.DropTable(
                name: "OperationTag");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "FinancialUnits");

            migrationBuilder.DropTable(
                name: "OperationActions");

            migrationBuilder.DropTable(
                name: "OperationEntries");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "AccountTypes");

            migrationBuilder.DropTable(
                name: "FinancialUnitTypes");

            migrationBuilder.DropTable(
                name: "Operations");
        }
    }
}
