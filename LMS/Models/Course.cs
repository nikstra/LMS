using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Course
    {
        public int      Id          { get; set; }
        public string   Name        { get; set; }
        public DateTime StartTime   { get; set; }
        public DateTime SlutTime    { get; set; }
        public int      ActivityId  { get; set; }

        public virtual ICollection<Group> Group { get; set; } // Course are enrolled to one Group.
    }
}