using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class publish2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Entries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 7, 15, 3, 24, 864, DateTimeKind.Local).AddTicks(3691),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 7, 14, 10, 27, 323, DateTimeKind.Local).AddTicks(1166));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 7, 15, 3, 24, 863, DateTimeKind.Local).AddTicks(7607),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 7, 14, 10, 27, 322, DateTimeKind.Local).AddTicks(4730));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Entries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 7, 14, 10, 27, 323, DateTimeKind.Local).AddTicks(1166),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 7, 15, 3, 24, 864, DateTimeKind.Local).AddTicks(3691));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 7, 14, 10, 27, 322, DateTimeKind.Local).AddTicks(4730),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 7, 15, 3, 24, 863, DateTimeKind.Local).AddTicks(7607));
        }
    }
}
