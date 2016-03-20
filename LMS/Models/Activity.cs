using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Activity : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Typ")]
        public ActivityType Type { get; set; }

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



        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

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

            if (CourseId > 0)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var parentCourse = db.Courses.Where(c => c.Id == CourseId).FirstOrDefault();

                if (!(StartDate >= parentCourse.StartDate && EndDate <= parentCourse.EndDate))
                {
                    yield return new ValidationResult("Aktivitetsdatum ligger utanför tidsperioden för kursen.");
                }
            }
        }
    }
}