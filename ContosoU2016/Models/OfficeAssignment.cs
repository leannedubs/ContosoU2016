using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models
{
    public class OfficeAssignment
    {
        [Key]
        [ForeignKey("Instructor")]
        public int InstructorID { get; set; }
        //  There is a 1 to 0 or 1 to 1 relationship between the instructor and the OfficeAssignment Entities

        //  An OfficeAssignment only exists in relation to the instructor it's assigned to, and therefore it's 
        //  primary key is also it's foreign key to the Instructor entity. 

        //  Problem:  Entity framework cannot automatically recognize InstructorID as the Primary key of this enitiy 
        //  because it's name doesn't follow the ID or ClassnameID naming convention. 

        // Therefore, the key attribute is used to identify it as the key. 

        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }


        // ---------- Navagation Properties ---------- //
        public virtual Instructor Instructor { get; set; }


    }
}
