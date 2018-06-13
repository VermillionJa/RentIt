﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RentIt.Data;
using RentIt.Data.Entities;
using System;

namespace RentIt.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RentIt.Data.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Address2");

                    b.Property<string>("City");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("State");

                    b.Property<string>("Zip");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("RentIt.Data.Entities.InventoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsCheckedOut");

                    b.Property<int?>("MovieId");

                    b.Property<int>("StorageFormat");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("RentIt.Data.Entities.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int?>("GenreId");

                    b.Property<int>("Rating");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("RentIt.Data.Entities.MovieGenre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("MovieGenres");
                });

            modelBuilder.Entity("RentIt.Data.Entities.RentalTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CustomerId");

                    b.Property<decimal>("LateFeesCharged");

                    b.Property<decimal>("RentalFeesCharged");

                    b.Property<decimal>("TotalAmountCharged");

                    b.Property<DateTime>("TransactionDate");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("RentalTransactions");
                });

            modelBuilder.Entity("RentIt.Data.Entities.RentalTransactionItems", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateReturned");

                    b.Property<DateTime>("DueDate");

                    b.Property<int?>("MovieId");

                    b.Property<int?>("TransactionId");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("TransactionId");

                    b.ToTable("RentalTransactionItems");
                });

            modelBuilder.Entity("RentIt.Data.Entities.InventoryItem", b =>
                {
                    b.HasOne("RentIt.Data.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("RentIt.Data.Entities.Movie", b =>
                {
                    b.HasOne("RentIt.Data.Entities.MovieGenre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId");
                });

            modelBuilder.Entity("RentIt.Data.Entities.RentalTransaction", b =>
                {
                    b.HasOne("RentIt.Data.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("RentIt.Data.Entities.RentalTransactionItems", b =>
                {
                    b.HasOne("RentIt.Data.Entities.InventoryItem", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");

                    b.HasOne("RentIt.Data.Entities.RentalTransaction", "Transaction")
                        .WithMany("Items")
                        .HasForeignKey("TransactionId");
                });
#pragma warning restore 612, 618
        }
    }
}
