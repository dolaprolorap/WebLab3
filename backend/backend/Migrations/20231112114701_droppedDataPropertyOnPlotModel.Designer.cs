﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using backend.DataAccess;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231112114701_droppedDataPropertyOnPlotModel")]
    partial class droppedDataPropertyOnPlotModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("backend.Models.DB.Plot", b =>
                {
                    b.Property<Guid>("PlotId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("PlotName")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("PlotId");

                    b.HasIndex("UserId");

                    b.ToTable("Plots");
                });

            modelBuilder.Entity("backend.Models.DB.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.HasKey("UserId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("32e8110c-b664-4a8b-b69d-f59fba269314"),
                            Password = "1111",
                            UserName = "RadiantDwarf"
                        },
                        new
                        {
                            UserId = new Guid("3b078e3f-e9a4-4098-adf2-ced840603bb4"),
                            Password = "2222",
                            UserName = "Dolaprolorap"
                        },
                        new
                        {
                            UserId = new Guid("5399ee18-ffdf-470b-bbae-160287b33244"),
                            Password = "3333",
                            UserName = "UltraGreed"
                        },
                        new
                        {
                            UserId = new Guid("da50fb97-6cca-4b03-af03-2d34ef433d59"),
                            Password = "4444",
                            UserName = "Reveqqq"
                        });
                });

            modelBuilder.Entity("backend.Models.DB.Plot", b =>
                {
                    b.HasOne("backend.Models.DB.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}