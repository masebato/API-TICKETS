using System;
using System.Collections.Generic;
using API_TICKETS.Models;
using Microsoft.EntityFrameworkCore;

namespace API_TICKETS.Data;

public partial class TicketapiContext : DbContext
{
    public TicketapiContext()
    {
    }

    public TicketapiContext(DbContextOptions<TicketapiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.IdTicket).HasName("PK__TICKET__22B1456FF0326012");

            entity.ToTable("TICKET");

            entity.Property(e => e.IdTicket).HasColumnName("idTicket");
            entity.Property(e => e.DateCreate)
                .HasColumnType("datetime")
                .HasColumnName("dateCreate");
            entity.Property(e => e.DateUpdate)
                .HasColumnType("datetime")
                .HasColumnName("dateUpdate");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_IDUSER");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__USER__3717C98228319CCE");

            entity.ToTable("USER");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
