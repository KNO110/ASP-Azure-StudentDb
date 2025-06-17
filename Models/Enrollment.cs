using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace StudentPortal.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        [Display(Name = "Студент")]
        [Required]
        public int StudentId { get; set; }

        [ValidateNever]
        public Student? Student { get; set; }

        [Display(Name = "Курс")]
        [Required]
        public int CourseId { get; set; }

        [ValidateNever]
        public Course? Course { get; set; }

        [Display(Name = "Оценка")]
        public int? Grade { get; set; }
    }
}
