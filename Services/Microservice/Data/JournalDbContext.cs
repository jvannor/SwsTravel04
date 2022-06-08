using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microservice.Models;

namespace Microservice.Data
{
    public partial class JournalDbContext : DbContext
    {
        public JournalDbContext()
        {
        }

        public JournalDbContext(DbContextOptions<JournalDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Journal> Journals { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Journal>(entity =>
            {
                entity.ToTable("Journal");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
