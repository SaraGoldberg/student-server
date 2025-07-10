using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StudentManagement.Data
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // הגדרת מפתח ראשי
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);

            // הגדרת שדות נדרשים
            modelBuilder.Entity<Student>()
                .Property(s => s.FullName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Student>()
                .Property(s => s.BirthDate)
                .IsRequired();

            // נתונים דמה למטרות בדיקה
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    StudentId = 1,
                    FullName = "יוסי כהן",
                    BirthDate = new DateTime(1995, 5, 15),
                    AverageGrade = 88.5,
                    IsActive = true
                },
                new Student
                {
                    StudentId = 2,
                    FullName = "רינה לוי",
                    BirthDate = new DateTime(1997, 8, 22),
                    AverageGrade = 92.0,
                    IsActive = true
                },
                new Student
                {
                    StudentId = 3,
                    FullName = "דני אברמוביץ",
                    BirthDate = new DateTime(1996, 3, 10),
                    AverageGrade = 75.5,
                    IsActive = false
                }
            );
        }
    }
}
