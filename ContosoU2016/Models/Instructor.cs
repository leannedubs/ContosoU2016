using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models
{
    public class Instructor : Person
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        // ---------- Navigation Propertity ---------- //


        // and instructor can teach any number of course, so courses is defined as a collection of the courseassigenment entity.
        public virtual ICollection<CourseAssignment> Courses { get; set; }


        // an instructor can only have at most one office, so the officeassignment property holds a single office assignment
        // which may be null fi there is no office assigned. 
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}
