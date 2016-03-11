using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public enum Role
    {
        Student,
        [Display(Name = "Lärare")]
        Teacher
    }
}