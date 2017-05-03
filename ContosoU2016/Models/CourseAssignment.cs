using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models
{
    public class CourseAssignment
    {
        public int InstructorID { get; set; } // Composite PK, FK to Instructor Entity

        public int CourseID { get; set; } // Composite PK, FK to Course Entity

        //  We could label both properties with the [Key] attribute to create a composite PK 
        //  but instead we will do it using Fluent-API within the SchoolContext


        // ---------- Navagation Properties ---------- 

            // Many to Many (this is the junction or join table) between Instructor and Course
            // Many Instructors teaching many courses
            // 1 course many course assignments
            // 1 instructor many course assignments

        public virtual Instructor Instructor { get; set; }

        public virtual Course Course { get; set; }
    }
}
