﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Visma.Timelogger.Persistence;

#nullable disable

namespace Visma.Timelogger.Persistence.Migrations
{
    [DbContext(typeof(ProjectDbContext))]
    [Migration("20240303010156_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Visma.Timelogger.Domain.Entities.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FreelancerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Projects");

                    b.HasData(
                        new
                        {
                            Id = new Guid("cb92d8fc-f475-4325-894f-36bdf9e863f2"),
                            CustomerId = new Guid("ae1e5c8e-7106-401a-a51f-fbec70972dee"),
                            Deadline = new DateTime(2024, 4, 3, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(6942),
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            IsCompleted = false,
                            StartTime = new DateTime(2024, 1, 17, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(6942)
                        },
                        new
                        {
                            Id = new Guid("6687be88-5db7-40be-9f67-870f3a4e1308"),
                            CustomerId = new Guid("671758f5-320d-4d1c-8c1a-54cdc55f2f75"),
                            Deadline = new DateTime(2024, 4, 13, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7032),
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            IsCompleted = false,
                            StartTime = new DateTime(2024, 2, 5, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7032)
                        },
                        new
                        {
                            Id = new Guid("3964e0d5-e99b-49fb-a07a-2b7896031a21"),
                            CustomerId = new Guid("671758f5-320d-4d1c-8c1a-54cdc55f2f75"),
                            Deadline = new DateTime(2024, 4, 26, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7043),
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            IsCompleted = true,
                            StartTime = new DateTime(2024, 2, 26, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7043)
                        },
                        new
                        {
                            Id = new Guid("bebc3044-7bfa-423b-87b7-dd48ade986b6"),
                            CustomerId = new Guid("ae1e5c8e-7106-401a-a51f-fbec70972dee"),
                            Deadline = new DateTime(2024, 5, 26, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7061),
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            IsCompleted = false,
                            StartTime = new DateTime(2024, 3, 16, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7061)
                        },
                        new
                        {
                            Id = new Guid("7e59fc0d-d11f-4d90-b389-6765eafc708f"),
                            CustomerId = new Guid("ae1e5c8e-7106-401a-a51f-fbec70972dee"),
                            Deadline = new DateTime(2024, 2, 2, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7074),
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            IsCompleted = false,
                            StartTime = new DateTime(2024, 1, 30, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7074)
                        },
                        new
                        {
                            Id = new Guid("345df3ad-8bf2-435c-b372-918ae695dbe1"),
                            CustomerId = new Guid("671758f5-320d-4d1c-8c1a-54cdc55f2f75"),
                            Deadline = new DateTime(2024, 3, 20, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7077),
                            FreelancerId = new Guid("486e3e8f-0dc3-4e00-8711-be3a6cb1399e"),
                            IsCompleted = true,
                            StartTime = new DateTime(2024, 2, 9, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7077)
                        },
                        new
                        {
                            Id = new Guid("effae1f0-514e-43f1-b0f5-2a93858bb3fa"),
                            CustomerId = new Guid("671758f5-320d-4d1c-8c1a-54cdc55f2f75"),
                            Deadline = new DateTime(2024, 3, 6, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7098),
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            IsCompleted = false,
                            StartTime = new DateTime(2024, 2, 2, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7098)
                        },
                        new
                        {
                            Id = new Guid("326ee974-6e29-4ed6-a48f-1669adafec95"),
                            CustomerId = new Guid("ae1e5c8e-7106-401a-a51f-fbec70972dee"),
                            Deadline = new DateTime(2024, 3, 8, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7108),
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            IsCompleted = true,
                            StartTime = new DateTime(2024, 2, 13, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7108)
                        },
                        new
                        {
                            Id = new Guid("da97f8a0-3703-4b44-9edc-4f854ff8ae0e"),
                            CustomerId = new Guid("ae1e5c8e-7106-401a-a51f-fbec70972dee"),
                            Deadline = new DateTime(2024, 5, 15, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7117),
                            FreelancerId = new Guid("486e3e8f-0dc3-4e00-8711-be3a6cb1399e"),
                            IsCompleted = false,
                            StartTime = new DateTime(2024, 2, 29, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7117)
                        },
                        new
                        {
                            Id = new Guid("1b2727e2-6fcc-4d03-8446-5cfc336540d4"),
                            CustomerId = new Guid("ae1e5c8e-7106-401a-a51f-fbec70972dee"),
                            Deadline = new DateTime(2024, 4, 29, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7145),
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            IsCompleted = false,
                            StartTime = new DateTime(2024, 2, 10, 2, 1, 56, 565, DateTimeKind.Local).AddTicks(7145)
                        });
                });

            modelBuilder.Entity("Visma.Timelogger.Domain.Entities.TimeRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DurationMinutes")
                        .HasColumnType("int");

                    b.Property<Guid>("FreelancerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("TimeRecords");

                    b.HasData(
                        new
                        {
                            Id = new Guid("2f0a5a28-3bd5-4c1e-b632-390c7ad314dc"),
                            DurationMinutes = 146,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("cb92d8fc-f475-4325-894f-36bdf9e863f2"),
                            StartTime = new DateTime(2024, 3, 11, 18, 42, 3, 470, DateTimeKind.Unspecified).AddTicks(8169)
                        },
                        new
                        {
                            Id = new Guid("b81a307f-0fe2-40a3-a188-b4bc69224678"),
                            DurationMinutes = 113,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("cb92d8fc-f475-4325-894f-36bdf9e863f2"),
                            StartTime = new DateTime(2024, 4, 2, 17, 1, 34, 355, DateTimeKind.Unspecified).AddTicks(6235)
                        },
                        new
                        {
                            Id = new Guid("cf5b6761-462f-4d46-896e-744d40a00695"),
                            DurationMinutes = 168,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("cb92d8fc-f475-4325-894f-36bdf9e863f2"),
                            StartTime = new DateTime(2024, 1, 21, 15, 7, 45, 631, DateTimeKind.Unspecified).AddTicks(8754)
                        },
                        new
                        {
                            Id = new Guid("af6ae859-e5fc-456b-8c91-09830ba570bf"),
                            DurationMinutes = 42,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("cb92d8fc-f475-4325-894f-36bdf9e863f2"),
                            StartTime = new DateTime(2024, 2, 20, 13, 34, 42, 429, DateTimeKind.Unspecified).AddTicks(8989)
                        },
                        new
                        {
                            Id = new Guid("e9117fd0-2233-486a-98c3-d1a05dbf29db"),
                            DurationMinutes = 116,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("6687be88-5db7-40be-9f67-870f3a4e1308"),
                            StartTime = new DateTime(2024, 2, 26, 6, 12, 27, 340, DateTimeKind.Unspecified).AddTicks(3383)
                        },
                        new
                        {
                            Id = new Guid("2839f657-162d-498b-801d-261b36a9c7f4"),
                            DurationMinutes = 109,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("6687be88-5db7-40be-9f67-870f3a4e1308"),
                            StartTime = new DateTime(2024, 2, 12, 9, 7, 48, 491, DateTimeKind.Unspecified).AddTicks(1643)
                        },
                        new
                        {
                            Id = new Guid("ead600bc-b1d1-492e-945e-550b8237560e"),
                            DurationMinutes = 45,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("3964e0d5-e99b-49fb-a07a-2b7896031a21"),
                            StartTime = new DateTime(2024, 4, 12, 7, 58, 25, 654, DateTimeKind.Unspecified).AddTicks(2701)
                        },
                        new
                        {
                            Id = new Guid("7ed6ee03-d02e-4c94-bd31-487ee329cee0"),
                            DurationMinutes = 71,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("3964e0d5-e99b-49fb-a07a-2b7896031a21"),
                            StartTime = new DateTime(2024, 4, 3, 19, 11, 10, 705, DateTimeKind.Unspecified).AddTicks(5982)
                        },
                        new
                        {
                            Id = new Guid("cb8e6784-80a8-4068-8624-1fb8c7301ea3"),
                            DurationMinutes = 121,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("3964e0d5-e99b-49fb-a07a-2b7896031a21"),
                            StartTime = new DateTime(2024, 3, 15, 9, 37, 8, 658, DateTimeKind.Unspecified).AddTicks(3252)
                        },
                        new
                        {
                            Id = new Guid("b8dde141-90fa-43b7-93ee-564d5b49b4fd"),
                            DurationMinutes = 80,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("3964e0d5-e99b-49fb-a07a-2b7896031a21"),
                            StartTime = new DateTime(2024, 3, 16, 0, 57, 43, 953, DateTimeKind.Unspecified).AddTicks(8412)
                        },
                        new
                        {
                            Id = new Guid("fdf86431-9ab1-41e7-aa81-95064e2c2f43"),
                            DurationMinutes = 80,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("3964e0d5-e99b-49fb-a07a-2b7896031a21"),
                            StartTime = new DateTime(2024, 4, 16, 8, 54, 9, 848, DateTimeKind.Unspecified).AddTicks(7157)
                        },
                        new
                        {
                            Id = new Guid("91060ceb-cb65-4bde-b982-649bdbb15a32"),
                            DurationMinutes = 30,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("bebc3044-7bfa-423b-87b7-dd48ade986b6"),
                            StartTime = new DateTime(2024, 4, 6, 15, 0, 19, 655, DateTimeKind.Unspecified).AddTicks(9537)
                        },
                        new
                        {
                            Id = new Guid("c3a08b01-d026-498e-9470-b632ac052bfd"),
                            DurationMinutes = 51,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("bebc3044-7bfa-423b-87b7-dd48ade986b6"),
                            StartTime = new DateTime(2024, 4, 7, 12, 18, 31, 773, DateTimeKind.Unspecified).AddTicks(8547)
                        },
                        new
                        {
                            Id = new Guid("e44499b2-dda6-4606-b956-b95790c26136"),
                            DurationMinutes = 38,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("bebc3044-7bfa-423b-87b7-dd48ade986b6"),
                            StartTime = new DateTime(2024, 5, 10, 16, 20, 43, 840, DateTimeKind.Unspecified).AddTicks(8024)
                        },
                        new
                        {
                            Id = new Guid("b28e0065-72fc-476a-86c4-b4f9d4c0c66a"),
                            DurationMinutes = 82,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("bebc3044-7bfa-423b-87b7-dd48ade986b6"),
                            StartTime = new DateTime(2024, 5, 16, 10, 30, 14, 575, DateTimeKind.Unspecified).AddTicks(8922)
                        },
                        new
                        {
                            Id = new Guid("35bd9072-4fd4-4920-889c-9d0782d71949"),
                            DurationMinutes = 35,
                            FreelancerId = new Guid("486e3e8f-0dc3-4e00-8711-be3a6cb1399e"),
                            ProjectId = new Guid("345df3ad-8bf2-435c-b372-918ae695dbe1"),
                            StartTime = new DateTime(2024, 3, 16, 19, 46, 6, 789, DateTimeKind.Unspecified).AddTicks(6735)
                        },
                        new
                        {
                            Id = new Guid("b47bc69c-e714-4377-8fcb-c70010883090"),
                            DurationMinutes = 188,
                            FreelancerId = new Guid("486e3e8f-0dc3-4e00-8711-be3a6cb1399e"),
                            ProjectId = new Guid("345df3ad-8bf2-435c-b372-918ae695dbe1"),
                            StartTime = new DateTime(2024, 2, 12, 22, 28, 48, 218, DateTimeKind.Unspecified).AddTicks(7197)
                        },
                        new
                        {
                            Id = new Guid("2e7c3a23-5810-46a4-8d53-274753fe083a"),
                            DurationMinutes = 59,
                            FreelancerId = new Guid("486e3e8f-0dc3-4e00-8711-be3a6cb1399e"),
                            ProjectId = new Guid("345df3ad-8bf2-435c-b372-918ae695dbe1"),
                            StartTime = new DateTime(2024, 3, 10, 3, 2, 12, 979, DateTimeKind.Unspecified).AddTicks(2023)
                        },
                        new
                        {
                            Id = new Guid("21ac7e7c-96d1-40a4-8180-1fcaaff4e2d7"),
                            DurationMinutes = 178,
                            FreelancerId = new Guid("486e3e8f-0dc3-4e00-8711-be3a6cb1399e"),
                            ProjectId = new Guid("345df3ad-8bf2-435c-b372-918ae695dbe1"),
                            StartTime = new DateTime(2024, 2, 13, 3, 49, 43, 125, DateTimeKind.Unspecified).AddTicks(755)
                        },
                        new
                        {
                            Id = new Guid("65caeaa0-fd0c-4c04-9b65-c0dc49d9e3db"),
                            DurationMinutes = 83,
                            FreelancerId = new Guid("486e3e8f-0dc3-4e00-8711-be3a6cb1399e"),
                            ProjectId = new Guid("345df3ad-8bf2-435c-b372-918ae695dbe1"),
                            StartTime = new DateTime(2024, 2, 21, 19, 44, 18, 166, DateTimeKind.Unspecified).AddTicks(2261)
                        },
                        new
                        {
                            Id = new Guid("39ff1aec-bdcf-4246-b3b3-1d2f9aad62a2"),
                            DurationMinutes = 184,
                            FreelancerId = new Guid("486e3e8f-0dc3-4e00-8711-be3a6cb1399e"),
                            ProjectId = new Guid("345df3ad-8bf2-435c-b372-918ae695dbe1"),
                            StartTime = new DateTime(2024, 2, 14, 13, 55, 47, 360, DateTimeKind.Unspecified).AddTicks(560)
                        },
                        new
                        {
                            Id = new Guid("08671c9d-0e57-4bca-afff-da5274aabb72"),
                            DurationMinutes = 108,
                            FreelancerId = new Guid("486e3e8f-0dc3-4e00-8711-be3a6cb1399e"),
                            ProjectId = new Guid("345df3ad-8bf2-435c-b372-918ae695dbe1"),
                            StartTime = new DateTime(2024, 3, 1, 3, 28, 11, 694, DateTimeKind.Unspecified).AddTicks(5982)
                        },
                        new
                        {
                            Id = new Guid("ab9224b3-06bb-430e-8a84-31959a60448f"),
                            DurationMinutes = 132,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("effae1f0-514e-43f1-b0f5-2a93858bb3fa"),
                            StartTime = new DateTime(2024, 3, 2, 17, 6, 24, 414, DateTimeKind.Unspecified).AddTicks(9604)
                        },
                        new
                        {
                            Id = new Guid("07cae022-92b3-45cb-947b-3eea42270d74"),
                            DurationMinutes = 144,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("effae1f0-514e-43f1-b0f5-2a93858bb3fa"),
                            StartTime = new DateTime(2024, 2, 3, 14, 7, 1, 490, DateTimeKind.Unspecified).AddTicks(8513)
                        },
                        new
                        {
                            Id = new Guid("0ff59b70-c7b9-438c-a197-e6828315571b"),
                            DurationMinutes = 161,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("effae1f0-514e-43f1-b0f5-2a93858bb3fa"),
                            StartTime = new DateTime(2024, 2, 27, 6, 28, 17, 838, DateTimeKind.Unspecified).AddTicks(6017)
                        },
                        new
                        {
                            Id = new Guid("1676b2f7-b1c3-4679-997f-db3bda251675"),
                            DurationMinutes = 145,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("326ee974-6e29-4ed6-a48f-1669adafec95"),
                            StartTime = new DateTime(2024, 2, 13, 21, 36, 18, 463, DateTimeKind.Unspecified).AddTicks(850)
                        },
                        new
                        {
                            Id = new Guid("7d8250a6-3a56-479f-8750-b43a7e74a38f"),
                            DurationMinutes = 50,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("326ee974-6e29-4ed6-a48f-1669adafec95"),
                            StartTime = new DateTime(2024, 2, 13, 10, 45, 53, 440, DateTimeKind.Unspecified).AddTicks(2603)
                        },
                        new
                        {
                            Id = new Guid("21b6605a-b415-4982-8009-41a511fd0583"),
                            DurationMinutes = 50,
                            FreelancerId = new Guid("486e3e8f-0dc3-4e00-8711-be3a6cb1399e"),
                            ProjectId = new Guid("da97f8a0-3703-4b44-9edc-4f854ff8ae0e"),
                            StartTime = new DateTime(2024, 3, 20, 7, 23, 6, 813, DateTimeKind.Unspecified).AddTicks(8729)
                        },
                        new
                        {
                            Id = new Guid("d686ba3d-a35a-4004-863b-0afb55c2a3fe"),
                            DurationMinutes = 156,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("1b2727e2-6fcc-4d03-8446-5cfc336540d4"),
                            StartTime = new DateTime(2024, 4, 1, 22, 1, 39, 626, DateTimeKind.Unspecified).AddTicks(2308)
                        },
                        new
                        {
                            Id = new Guid("f3044f8e-c979-4117-a9af-4c3fbcb19ef2"),
                            DurationMinutes = 165,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("1b2727e2-6fcc-4d03-8446-5cfc336540d4"),
                            StartTime = new DateTime(2024, 3, 31, 2, 10, 27, 981, DateTimeKind.Unspecified).AddTicks(7263)
                        },
                        new
                        {
                            Id = new Guid("3b04903e-7d9f-4ff7-bae0-34e540c2e24f"),
                            DurationMinutes = 167,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("1b2727e2-6fcc-4d03-8446-5cfc336540d4"),
                            StartTime = new DateTime(2024, 3, 19, 4, 30, 30, 310, DateTimeKind.Unspecified).AddTicks(6861)
                        },
                        new
                        {
                            Id = new Guid("a1424158-7166-4ddd-9f64-4cc53e54b7ae"),
                            DurationMinutes = 35,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("1b2727e2-6fcc-4d03-8446-5cfc336540d4"),
                            StartTime = new DateTime(2024, 3, 21, 17, 59, 17, 947, DateTimeKind.Unspecified).AddTicks(2606)
                        },
                        new
                        {
                            Id = new Guid("c7650e2d-b215-4644-9650-09aee643bb51"),
                            DurationMinutes = 74,
                            FreelancerId = new Guid("dd330056-ee5a-451b-ac2c-af0cb20eb213"),
                            ProjectId = new Guid("1b2727e2-6fcc-4d03-8446-5cfc336540d4"),
                            StartTime = new DateTime(2024, 2, 24, 14, 55, 34, 786, DateTimeKind.Unspecified).AddTicks(5878)
                        });
                });

            modelBuilder.Entity("Visma.Timelogger.Domain.Entities.TimeRecord", b =>
                {
                    b.HasOne("Visma.Timelogger.Domain.Entities.Project", null)
                        .WithMany("TimeRecords")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Visma.Timelogger.Domain.Entities.Project", b =>
                {
                    b.Navigation("TimeRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
