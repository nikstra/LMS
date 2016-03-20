using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Course : IValidatableObject
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

        public int GroupId { get; set; }
        public int? ActivityId { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }

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

            if (GroupId > 0)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var parentGroup = db.Groups.Where(g => g.Id == GroupId).FirstOrDefault();

                if (!(StartDate >= parentGroup.StartDate && EndDate <= parentGroup.EndDate))
                {
                    yield return new ValidationResult("Kursdatum ligger utanför tidsperioden för gruppen.");
                }
            }
        }
    }
}