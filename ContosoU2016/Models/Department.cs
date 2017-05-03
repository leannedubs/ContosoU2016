using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models
{
    public class Department
    {

        public int DepartmentID { get; set; } // PK

        [Required]
        [StringLength(50, MinimumLength =3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)] // client side
        [Column(TypeName = "money")] // database side 
        public decimal Budget { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }


        // relationship to instructor 
        // a department MAY have an administrator (and instructor with that title) and an instructor is always and instructor. 
        public int? InstructorID { get; set; } // MAY == nullable == int?


        // ---------- Navagation Properties ---------- //
        // Adminstator is always an instructor
        public virtual Instructor Administrator { get; set; }
    
        // one department has many courses
        public virtual ICollection<Course> Courses { get; set; }

        ///////////    TO DO :  Handle Concurrency conflicts  (add optimistic concurrency)
   
    }
}
