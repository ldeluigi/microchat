﻿// <auto-generated />
using System;
using ChatService.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatService.Infrastructure.DataAccess.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20220207204738_InitialState")]
    partial class InitialState
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("domain")
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ChatService.Infrastructure.DataAccess.Model.ChatAggregate.PrivateChatModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreationTime")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PartecipantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId", "PartecipantId")
                        .IsUnique()
                        .HasFilter("[CreatorId] IS NOT NULL AND [PartecipantId] IS NOT NULL");

                    b.HasIndex("PartecipantId", "CreatorId")
                        .IsUnique()
                        .HasFilter("[PartecipantId] IS NOT NULL AND [CreatorId] IS NOT NULL");

                    b.ToTable("PrivateChats", "domain");
                });

            modelBuilder.Entity("ChatService.Infrastructure.DataAccess.Model.MessageAggregate.PrivateMessageModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastEditTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("SendTime")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Viewed")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("ChatId");

                    b.HasIndex("SendTime");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("SendTime"));

                    b.ToTable("PrivateMessages", "domain");
                });

            modelBuilder.Entity("ChatService.Infrastructure.DataAccess.Model.UserAggregate.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Users", "domain");
                });
#pragma warning restore 612, 618
        }
    }
}