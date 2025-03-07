﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBorrow.Migrations
{
    /// <inheritdoc />
    public partial class afterborrowed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfCopiesAfterBorrowed",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfCopiesAfterBorrowed",
                table: "Books");
        }
    }
}
