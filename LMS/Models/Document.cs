using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Document
    {
        public int Id { get; set; }
        [Display(Name = "Dokumentnamn")]
        public string Name { get; set; }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        public string Feedback { get; set; }
        [Display(Name = "Tidsstämpel")]
        public DateTime TimeStamp { get; set; }
        public string LocalPath { get; set; }
        public int? ActivityId { get; set; }
        public string ApplicationUserId { get; set; }
        public int? CourseId { get; set; }
        public int? GroupId { get; set; }

        [Display(Name = "Aktivitet")]
        public virtual Activity Activity { get; set; }
        [Display(Name = "Kurs")]
        public virtual Course Course { get; set; }
        [Display(Name = "Grupp")]
        public virtual Group Group { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}