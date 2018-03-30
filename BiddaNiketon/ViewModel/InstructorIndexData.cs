using BiddaNiketon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiddaNiketon.ViewModel
{
    public class InstructorIndexData
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}