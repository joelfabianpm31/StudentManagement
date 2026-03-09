using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Cada DbSet representa una tabla en la base de datos
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<EnrollmentDetail> EnrollmentDetails => Set<EnrollmentDetail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Tabla Students ────────────────────────────────────────────
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.FirstName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(s => s.LastName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(s => s.Email)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(s => s.DocumentNumber)
                      .IsRequired()
                      .HasMaxLength(50);
                // Indices unicos para evitar duplicados
                entity.HasIndex(s => s.Email).IsUnique();
                entity.HasIndex(s => s.DocumentNumber).IsUnique();
                // Ignorar la propiedad calculada (no se guarda en la BD)
                entity.Ignore(s => s.FullName);
            });

            // ── Tabla Courses ─────────────────────────────────────────────
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(c => c.Code)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(c => c.Description)
                      .HasMaxLength(1000);
                entity.HasIndex(c => c.Code).IsUnique();
            });

            // ── Tabla Enrollments ─────────────────────────────────────────
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(en => en.Id);
                entity.Property(en => en.Status)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(en => en.Notes)
                      .HasMaxLength(500);
                // Relacion: un Student tiene muchos Enrollments
                entity.HasOne(en => en.Student)
                      .WithMany(s => s.Enrollments)
                      .HasForeignKey(en => en.StudentId)
                      .OnDelete(DeleteBehavior.Restrict); // No borrar cascada
            });

            // ── Tabla EnrollmentDetails ───────────────────────────────────
            modelBuilder.Entity<EnrollmentDetail>(entity =>
            {
                entity.HasKey(ed => ed.Id);
                entity.Property(ed => ed.Status)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(ed => ed.Grade)
                      .HasPrecision(5, 2); // Ej: 100.00
                                           // Relacion: un Enrollment tiene muchos EnrollmentDetails
                entity.HasOne(ed => ed.Enrollment)
                      .WithMany(en => en.EnrollmentDetails)
                      .HasForeignKey(ed => ed.EnrollmentId)
                      .OnDelete(DeleteBehavior.Cascade); // Borrar detalles si se borra la matricula
                                                         // Relacion: un Course aparece en muchos EnrollmentDetails
                entity.HasOne(ed => ed.Course)
                      .WithMany(c => c.EnrollmentDetails)
                      .HasForeignKey(ed => ed.CourseId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
