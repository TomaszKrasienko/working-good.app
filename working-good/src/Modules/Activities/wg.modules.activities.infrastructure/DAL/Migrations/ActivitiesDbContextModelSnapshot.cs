﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using wg.modules.activities.infrastructure.DAL;

#nullable disable

namespace wg.modules.activities.infrastructure.DAL.Migrations
{
    [DbContext(typeof(ActivitiesDbContext))]
    partial class ActivitiesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("activities")
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("wg.modules.activities.domain.Entities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DailyUserActivityDay")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DailyUserActivityUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.HasKey("Id");

                    b.HasIndex("DailyUserActivityDay", "DailyUserActivityUserId");

                    b.ToTable("Activities", "activities");

                    b.HasDiscriminator<string>("type").HasValue("Activity");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("wg.modules.activities.domain.Entities.DailyUserActivity", b =>
                {
                    b.Property<DateTime>("Day")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Day", "UserId");

                    b.ToTable("DailyUserActivities", "activities");
                });

            modelBuilder.Entity("wg.modules.activities.domain.Entities.InternalActivity", b =>
                {
                    b.HasBaseType("wg.modules.activities.domain.Entities.Activity");

                    b.HasDiscriminator().HasValue("InternalActivity");
                });

            modelBuilder.Entity("wg.modules.activities.domain.Entities.PaidActivity", b =>
                {
                    b.HasBaseType("wg.modules.activities.domain.Entities.Activity");

                    b.HasDiscriminator().HasValue("PaidActivity");
                });

            modelBuilder.Entity("wg.modules.activities.domain.Entities.Activity", b =>
                {
                    b.HasOne("wg.modules.activities.domain.Entities.DailyUserActivity", null)
                        .WithMany("Activities")
                        .HasForeignKey("DailyUserActivityDay", "DailyUserActivityUserId");

                    b.OwnsOne("wg.modules.activities.domain.ValueObjects.Activity.ActivityTime", "ActivityTime", b1 =>
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

                            b1.ToTable("Activities", "activities");

                            b1.WithOwner()
                                .HasForeignKey("ActivityId");
                        });

                    b.Navigation("ActivityTime");
                });

            modelBuilder.Entity("wg.modules.activities.domain.Entities.DailyUserActivity", b =>
                {
                    b.Navigation("Activities");
                });
#pragma warning restore 612, 618
        }
    }
}
