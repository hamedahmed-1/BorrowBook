﻿// <auto-generated />
using System;
using BookBorrow.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookBorrow.Migrations
{
    [DbContext(typeof(BookBorrowContext))]
    [Migration("20240802172604_validate no of copies ")]
    partial class validatenoofcopies
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookBorrow.Models.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"));

                    b.Property<string>("BookImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfCopies")
                        .HasColumnType("int");

                    b.HasKey("BookId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("BookBorrow.Models.BorrowRecord", b =>
                {
                    b.Property<int>("RecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecordId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BorrowDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.HasKey("RecordId");

                    b.HasIndex("BookId");

                    b.HasIndex("ClientId");

                    b.ToTable("BorrowRecords");
                });

            modelBuilder.Entity("BookBorrow.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientId"));

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientPhoneNumber")
                        .HasColumnType("int");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("BookBorrow.Models.BorrowRecord", b =>
                {
                    b.HasOne("BookBorrow.Models.Book", "Book")
                        .WithMany("BorrowRecords")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookBorrow.Models.Client", "Client")
                        .WithMany("BorrowRecords")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("BookBorrow.Models.Book", b =>
                {
                    b.Navigation("BorrowRecords");
                });

            modelBuilder.Entity("BookBorrow.Models.Client", b =>
                {
                    b.Navigation("BorrowRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
