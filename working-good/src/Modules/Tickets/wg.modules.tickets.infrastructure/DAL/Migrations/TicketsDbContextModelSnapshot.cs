﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using wg.modules.tickets.infrastructure.DAL;

#nullable disable

namespace wg.modules.tickets.infrastructure.DAL.Migrations
{
    [DbContext(typeof(TicketsDbContext))]
    partial class TicketsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("tickets")
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("wg.modules.tickets.domain.Entities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TicketId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("Activities", "tickets");
                });

            modelBuilder.Entity("wg.modules.tickets.domain.Entities.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Sender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<Guid?>("TicketId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("Messages", "tickets");
                });

            modelBuilder.Entity("wg.modules.tickets.domain.Entities.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AssignedEmployee")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AssignedUser")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPriority")
                        .HasColumnType("bit");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.ToTable("Tickets", "tickets");
                });

            modelBuilder.Entity("wg.modules.tickets.domain.Entities.Activity", b =>
                {
                    b.HasOne("wg.modules.tickets.domain.Entities.Ticket", null)
                        .WithMany("Activities")
                        .HasForeignKey("TicketId");

                    b.OwnsOne("wg.modules.tickets.domain.ValueObjects.Activity.ActivityTime", "ActivityTime", b1 =>
                        {
                            b1.Property<Guid>("ActivityId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("TimeFrom")
                                .HasColumnType("datetime2")
                                .HasColumnName("ActivityTimeFrom");

                            b1.Property<DateTime?>("TimeTo")
                                .HasColumnType("datetime2")
                                .HasColumnName("ActivityTimeTo");

                            b1.HasKey("ActivityId");

                            b1.ToTable("Activities", "tickets");

                            b1.WithOwner()
                                .HasForeignKey("ActivityId");
                        });

                    b.Navigation("ActivityTime");
                });

            modelBuilder.Entity("wg.modules.tickets.domain.Entities.Message", b =>
                {
                    b.HasOne("wg.modules.tickets.domain.Entities.Ticket", null)
                        .WithMany("Messages")
                        .HasForeignKey("TicketId");
                });

            modelBuilder.Entity("wg.modules.tickets.domain.Entities.Ticket", b =>
                {
                    b.OwnsOne("wg.modules.tickets.domain.ValueObjects.Ticket.State", "State", b1 =>
                        {
                            b1.Property<Guid>("TicketId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("ChangeDate")
                                .HasColumnType("datetime2")
                                .HasColumnName("StateChangeDate");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(40)
                                .HasColumnType("nvarchar(40)")
                                .HasColumnName("State");

                            b1.HasKey("TicketId");

                            b1.ToTable("Tickets", "tickets");

                            b1.WithOwner()
                                .HasForeignKey("TicketId");
                        });

                    b.Navigation("State");
                });

            modelBuilder.Entity("wg.modules.tickets.domain.Entities.Ticket", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
