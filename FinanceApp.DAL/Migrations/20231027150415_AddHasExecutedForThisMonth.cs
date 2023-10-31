using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddHasExecutedForThisMonth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Entries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 27, 18, 4, 15, 140, DateTimeKind.Local).AddTicks(6988),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 26, 20, 22, 48, 164, DateTimeKind.Local).AddTicks(8125));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 27, 18, 4, 15, 140, DateTimeKind.Local).AddTicks(4407),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 26, 20, 22, 48, 164, DateTimeKind.Local).AddTicks(4262));

            migrationBuilder.AddColumn<bool>(
                name: "HasExecutedForThisMonth",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasExecutedForThisMonth",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Entries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 26, 20, 22, 48, 164, DateTimeKind.Local).AddTicks(8125),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 27, 18, 4, 15, 140, DateTimeKind.Local).AddTicks(6988));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 26, 20, 22, 48, 164, DateTimeKind.Local).AddTicks(4262),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 27, 18, 4, 15, 140, DateTimeKind.Local).AddTicks(4407));
        }
    }
}
