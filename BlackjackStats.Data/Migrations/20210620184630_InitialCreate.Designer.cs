﻿// <auto-generated />
using System;
using BlackjackStats.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlackjackStats.Data.Migrations
{
    [DbContext(typeof(BlackjackStatsContext))]
    [Migration("20210620184630_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BlackjackStats.Data.Entities.Deal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("Card")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("GameID")
                        .HasColumnType("int");

                    b.Property<string>("Outcome")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Player")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<int>("SplitScore")
                        .HasColumnType("int");

                    b.HasKey("ID")
                        .HasName("DealID");

                    b.HasIndex("GameID");

                    b.ToTable("Deal");
                });

            modelBuilder.Entity("BlackjackStats.Data.Entities.Game", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Ip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ID")
                        .HasName("GameID");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("BlackjackStats.Data.Entities.Deal", b =>
                {
                    b.HasOne("BlackjackStats.Data.Entities.Game", "Game")
                        .WithMany("Deals")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("BlackjackStats.Data.Entities.Game", b =>
                {
                    b.Navigation("Deals");
                });
#pragma warning restore 612, 618
        }
    }
}