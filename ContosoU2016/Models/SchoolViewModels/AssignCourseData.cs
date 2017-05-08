using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models.SchoolViewModels
{
    public class AssignCourseData
    {
        // to provide a list of course check boxes with courseId and title as well as an indicator that the instructor is assigned 
        // or not assigned to a particular course. We are creating this ViewModel class. 

        public int CourseID { get; set; }
        public string Title { get; set; }
        public bool Assigned { get; set; }
    }
}
