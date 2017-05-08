using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models.SchoolViewModels
{
    public class EnrollmentDateGroup
    {
        //  This class is considered to be a ViewModel. A ViewModel allows you to shape multiple entities into a single object, 
        //  optimized for consumption and rendering by the view.  The purpose of the ViewModel is for the ciew to have a single
        //  object to render.

        
        [DataType(DataType.Date)] 
        public DateTime? EnrollmentDate { get; set; }

        public int StudentCount { get; set; }
    }
}
