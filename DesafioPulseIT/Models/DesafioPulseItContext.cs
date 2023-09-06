using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DesafioPulseIT.Models;

public partial class DesafioPulseItContext : DbContext
{
    public DesafioPulseItContext()
    {
    }

    public DesafioPulseItContext(DbContextOptions<DesafioPulseItContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Invitado> Invitados { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=OVERLEG;Database=desafioPulseIT;Integrated Security=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Eventos__3213E83F0E074274");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(128)
                .HasColumnName("descripcion");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Titulo)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("titulo");
        });

        modelBuilder.Entity<Invitado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Invitado__3213E83F99BE57F5");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Asiste).HasColumnName("asiste");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EventoId).HasColumnName("eventoID");
            entity.Property(e => e.Respondio).HasColumnName("respondio");

            entity.HasOne(d => d.Evento).WithMany(p => p.Invitados)
                .HasForeignKey(d => d.EventoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invitados_eventos");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
