using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Range(0, 100)]
        public double AverageGrade { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
