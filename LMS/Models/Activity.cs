using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Activity
    {
        public int      Id          { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        public string   Description { get; set; }

        [Display(Name = "Start datum")]
        public DateTime StartDate   { get; set; }

        [Display(Name = "Slut datum")]
        public DateTime EndDate    { get; set; }



        public int      CourseId    { get; set; }
    }
}