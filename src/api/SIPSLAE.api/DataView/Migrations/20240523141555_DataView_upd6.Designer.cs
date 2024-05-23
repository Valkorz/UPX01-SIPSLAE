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
    [Migration("20240523141555_DataView_upd6")]
    partial class DataView_upd6
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

                    b.Property<bool>("IsRaining")
                        .HasColumnType("INTEGER");

                    b.Property<double>("MinuteInterval")
                        .HasColumnType("REAL");

                    b.Property<int>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("TimeOfRecord")
                        .HasColumnType("TEXT");

                    b.Property<float>("Variation")
                        .HasColumnType("REAL");

                    b.Property<float>("WaterLevel")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("logs");
                });
#pragma warning restore 612, 618
        }
    }
}
