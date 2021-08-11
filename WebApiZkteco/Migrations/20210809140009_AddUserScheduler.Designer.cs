﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiZkteco.Models;

namespace WebApiZkteco.Migrations
{
    [DbContext(typeof(ZkContext))]
    [Migration("20210809140009_AddUserScheduler")]
    partial class AddUserScheduler
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApiZkteco.Models.User", b =>
                {
                    b.Property<string>("sUserID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("activeStart")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("activeStop")
                        .HasColumnType("datetime2");

                    b.Property<bool>("bEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("iFaceIndex")
                        .HasColumnType("int");

                    b.Property<int>("iFaceLen")
                        .HasColumnType("int");

                    b.Property<int>("iFingerFlag")
                        .HasColumnType("int");

                    b.Property<int>("iFingerLen")
                        .HasColumnType("int");

                    b.Property<int>("iPrivilege")
                        .HasColumnType("int");

                    b.Property<int>("idwFingerIndex")
                        .HasColumnType("int");

                    b.Property<string>("sFaceData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sFingerData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sPassword")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("sUserID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}