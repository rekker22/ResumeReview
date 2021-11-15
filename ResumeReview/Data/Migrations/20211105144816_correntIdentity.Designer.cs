﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResumeReview.Data;

namespace ResumeReview.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211105144816_correntIdentity")]
    partial class correntIdentity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ResumeReview.Models.Resume", b =>
                {
                    b.Property<int>("ResumeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ResumeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UploaderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Uri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VersionNumber")
                        .HasColumnType("int");

                    b.HasKey("ResumeId");

                    b.ToTable("Resume");
                });

            modelBuilder.Entity("ResumeReview.Models.Reviews", b =>
                {
                    b.Property<int>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("ResumeId")
                        .HasColumnType("int");

                    b.Property<string>("Review")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReviewDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ReviewerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ReviewId");

                    b.HasIndex("ResumeId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("ResumeReview.Models.UserViewed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ResumeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UserViewedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserViewedId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ResumeId");

                    b.ToTable("UserViewed");
                });

            modelBuilder.Entity("ResumeReview.Models.Reviews", b =>
                {
                    b.HasOne("ResumeReview.Models.Resume", "Resume")
                        .WithMany()
                        .HasForeignKey("ResumeId");

                    b.Navigation("Resume");
                });

            modelBuilder.Entity("ResumeReview.Models.UserViewed", b =>
                {
                    b.HasOne("ResumeReview.Models.Resume", "Resume")
                        .WithMany()
                        .HasForeignKey("ResumeId");

                    b.Navigation("Resume");
                });
#pragma warning restore 612, 618
        }
    }
}
