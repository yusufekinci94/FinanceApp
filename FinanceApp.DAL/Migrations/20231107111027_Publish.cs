using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Publish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Entries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 7, 14, 10, 27, 323, DateTimeKind.Local).AddTicks(1166),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 2, 1, 0, 45, 179, DateTimeKind.Local).AddTicks(6333));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 7, 14, 10, 27, 322, DateTimeKind.Local).AddTicks(4730),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 2, 1, 0, 45, 179, DateTimeKind.Local).AddTicks(3743));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Entries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 2, 1, 0, 45, 179, DateTimeKind.Local).AddTicks(6333),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 7, 14, 10, 27, 323, DateTimeKind.Local).AddTicks(1166));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 2, 1, 0, 45, 179, DateTimeKind.Local).AddTicks(3743),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 7, 14, 10, 27, 322, DateTimeKind.Local).AddTicks(4730));
        }
    }
}
