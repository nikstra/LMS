using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Group : IValidatableObject
    {

        public int Id { get; set; }
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [Display(Name = "Beskrivning")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Startdatum")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "Slutdatum")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int? CourseId { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult("Slutdatum måste vara senare än startdatum.");
            }

            //if (StartDate < DateTime.Now.Date)
            //{
            //    yield return new ValidationResult("Startdatum har passerat.");
            //}
        }
    }
}