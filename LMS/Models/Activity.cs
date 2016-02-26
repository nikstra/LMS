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
        public DateTime StartDate   { get; set; }
        public DateTime SlutDate    { get; set; }
        public string   Description { get; set; }
        public int      CourseId    { get; set; }

        public virtual Course Course { get; set; }
    }
}