using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public enum ActivityType
    {
        [Display(Name = "Inlämningsuppgift")]
        Assignment,
        [Display(Name = "Code-along")]
        Codealong,
        [Display(Name = "E-learning")]
        Elearning,
        [Display(Name = "Övningsuppgift")]
        Excercise,
        [Display(Name = "Föreläsning")]
        Lecture
    }
}