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
        public DateTime StartDate   { get; set; }
        public DateTime EndDate     { get; set; }
        public int      GroupId     { get; set; }
        public int?     ActivityId  { get; set; }
        public string   Description { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
}