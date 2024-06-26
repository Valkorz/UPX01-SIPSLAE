﻿// <auto-generated />
using System;
using DataView.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataView.Migrations
{
    [DbContext(typeof(MonitorContext))]
    [Migration("20240522152157_Data_View2205")]
    partial class Data_View2205
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("DataView.Controllers.MonitorBacklog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("TimeOfRecord")
                        .HasColumnType("TEXT");

                    b.Property<float>("WaterLevel")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("logs");
                });
#pragma warning restore 612, 618
        }
    }
}
