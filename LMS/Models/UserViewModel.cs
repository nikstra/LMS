using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class UserViewModel
    {
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Display(Name = "E-post")]
        public string Email { get; set; }
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Roll")]
        public string RoleName { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }


}