﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Suffer_Travels.Data;

#nullable disable

namespace Suffer_Travels.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220216170441_AddCityTourPhotoToDatabase")]
    partial class AddCityTourPhotoToDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Suffer_Travels.Models.Area", b =>
                {
                    b.Property<long>("AId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("AId"), 1L, 1);

                    b.Property<string>("Aname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("CityId")
                        .HasColumnType("bigint");

                    b.Property<long>("Pincode")
                        .HasColumnType("bigint");

                    b.HasKey("AId");

                    b.ToTable("tblArea");
                });

            modelBuilder.Entity("Suffer_Travels.Models.City", b =>
                {
                    b.Property<long>("CId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CId"), 1L, 1);

                    b.Property<string>("Cname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("StateId")
                        .HasColumnType("bigint");

                    b.HasKey("CId");

                    b.ToTable("tblCity");
                });

            modelBuilder.Entity("Suffer_Travels.Models.Country", b =>
                {
                    b.Property<long>("CId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CId"), 1L, 1);

                    b.Property<string>("Cname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("STDcode")
                        .HasColumnType("int");

                    b.HasKey("CId");

                    b.ToTable("tblCountry");
                });

            modelBuilder.Entity("Suffer_Travels.Models.Photo", b =>
                {
                    b.Property<long>("PId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("PId"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PId");

                    b.ToTable("tblPhotos");
                });

            modelBuilder.Entity("Suffer_Travels.Models.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblRole");
                });

            modelBuilder.Entity("Suffer_Travels.Models.State", b =>
                {
                    b.Property<long>("SId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("SId"), 1L, 1);

                    b.Property<long>("CountryId")
                        .HasColumnType("bigint");

                    b.Property<string>("Sname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SId");

                    b.ToTable("tblState");
                });

            modelBuilder.Entity("Suffer_Travels.Models.Tour", b =>
                {
                    b.Property<long>("TId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TId"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("NoOfDays")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("TotalSeats")
                        .HasColumnType("int");

                    b.Property<string>("TourName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TourTypeId")
                        .HasColumnType("bigint");

                    b.HasKey("TId");

                    b.ToTable("tblTour");
                });

            modelBuilder.Entity("Suffer_Travels.Models.TourCities", b =>
                {
                    b.Property<long>("TcId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TcId"), 1L, 1);

                    b.Property<long>("CityId")
                        .HasColumnType("bigint");

                    b.Property<long>("TourId")
                        .HasColumnType("bigint");

                    b.HasKey("TcId");

                    b.ToTable("tblTourCities");
                });

            modelBuilder.Entity("Suffer_Travels.Models.TourDates", b =>
                {
                    b.Property<long>("TdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TdId"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<long>("TourId")
                        .HasColumnType("bigint");

                    b.HasKey("TdId");

                    b.ToTable("tblTourDates");
                });

            modelBuilder.Entity("Suffer_Travels.Models.TourItinerary", b =>
                {
                    b.Property<long>("TiId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TiId"), 1L, 1);

                    b.Property<long>("Day")
                        .HasColumnType("bigint");

                    b.Property<long>("LandmarkId")
                        .HasColumnType("bigint");

                    b.Property<long>("MealComboId")
                        .HasColumnType("bigint");

                    b.Property<long>("TourId")
                        .HasColumnType("bigint");

                    b.HasKey("TiId");

                    b.ToTable("tblTourItinerary");
                });

            modelBuilder.Entity("Suffer_Travels.Models.TourPhotos", b =>
                {
                    b.Property<long>("TpId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TpId"), 1L, 1);

                    b.Property<long>("PhotoId")
                        .HasColumnType("bigint");

                    b.Property<long>("TourId")
                        .HasColumnType("bigint");

                    b.HasKey("TpId");

                    b.ToTable("tblTourPhotos");
                });

            modelBuilder.Entity("Suffer_Travels.Models.TourType", b =>
                {
                    b.Property<long>("TtId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TtId"), 1L, 1);

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TtName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TtId");

                    b.ToTable("tblTourType");
                });

            modelBuilder.Entity("Suffer_Travels.Models.User", b =>
                {
                    b.Property<long>("UId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("UId"), 1L, 1);

                    b.Property<DateTime?>("ChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ContactNo")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("Date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Lname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ProfilePhoto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UId");

                    b.ToTable("tblUser");
                });
#pragma warning restore 612, 618
        }
    }
}