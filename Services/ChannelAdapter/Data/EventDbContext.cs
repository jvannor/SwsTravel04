using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ChannelAdapter.Models;

namespace ChannelAdapter.Data
{
    public partial class EventDbContext : DbContext
    {
        public EventDbContext()
        {
        }

        public EventDbContext(DbContextOptions<EventDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Events { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.Completed).HasColumnType("datetime");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EventCode).HasMaxLength(64);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Scheduled)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
