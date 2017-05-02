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
        //public virtual ICollection<CourseAssignment> Courses { get; set; }
        //public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}
