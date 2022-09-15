﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(GamingStoreContext))]
    [Migration("20220915144802_AddAdminEmail")]
    partial class AddAdminEmail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Core.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Core.Entities.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Category")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("InStock")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(8,2)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@gmail.com",
                            PasswordHash = new byte[] { 153, 204, 169, 51, 26, 103, 176, 250, 127, 168, 69, 17, 240, 117, 162, 148, 59, 148, 206, 253, 129, 13, 199, 15, 135, 74, 235, 197, 19, 231, 111, 42, 25, 126, 35, 51, 37, 196, 235, 71, 160, 42, 107, 213, 213, 183, 56, 130, 49, 62, 45, 70, 5, 187, 174, 134, 42, 63, 51, 234, 62, 249, 235, 64 },
                            PasswordSalt = new byte[] { 236, 134, 230, 170, 253, 65, 212, 224, 74, 66, 7, 102, 60, 62, 205, 166, 196, 68, 141, 63, 123, 24, 79, 170, 156, 65, 29, 24, 240, 85, 136, 11, 232, 158, 14, 112, 186, 97, 218, 120, 68, 38, 150, 141, 159, 153, 20, 131, 55, 164, 246, 82, 250, 8, 243, 247, 88, 151, 130, 2, 134, 191, 125, 204, 67, 31, 91, 60, 115, 99, 184, 124, 255, 109, 46, 125, 182, 216, 116, 100, 124, 159, 85, 82, 156, 61, 163, 245, 129, 180, 232, 157, 140, 65, 141, 71, 183, 196, 108, 164, 43, 113, 173, 4, 57, 45, 92, 228, 133, 109, 64, 235, 233, 177, 24, 100, 93, 195, 74, 223, 39, 22, 78, 168, 226, 48, 152, 156 },
                            Role = 0,
                            Username = "Admin"
                        });
                });

            modelBuilder.Entity("Core.Entities.Device", b =>
                {
                    b.HasOne("Core.Entities.Company", "Company")
                        .WithMany("Devices")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Core.Entities.Company", b =>
                {
                    b.Navigation("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}