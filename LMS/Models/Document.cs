using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Feedback { get; set; }
        public DateTime TimeStamp { get; set; }
        public string LocalPath { get; set; }
        public int? ActivityId { get; set; }
        public string ApplicationUserId { get; set; }
        public int? CourseId { get; set; }
        public int? GroupId { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Course Course { get; set; }
        public virtual Group Group { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}