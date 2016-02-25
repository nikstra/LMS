using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Activity
    {
        public int      Id          { get; set; }
        public string   Name        { get; set; }
        public DateTime StartTime   { get; set; }
        public DateTime SlutTime    { get; set; }
        public string   Description { get; set; }

        public virtual ICollection<Course> Course { get; set; } // Activities are enrolled to one kurs.
    }
}