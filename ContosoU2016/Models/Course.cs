using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoU2016.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        // you can turn off the Identity Property (auto incrimenting) by using the DatabaseGeneratedOption.None
        // you have the following three options: 
             // Computed: Database generates a vaule when a row is inserted or updated. 
             // Identy: Database generates a vaule when a row is inserted.
             // None: Database does not generate a value.
            
        [Display(Name ="Course Number")]
        public int CourseID { get; set; } //PK

        [StringLength(50, MinimumLength =3)]
        [Required]
        public string Title { get; set; }

        [Range(0,5)]
        public int Credits { get; set; }

        [Display(Name ="Department ID")]
        public int DepartmentID { get; set; }

        // ---------- Navigation Properties ---------- //
        // 1 course, many enrollments 
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        // 1 course, many instructors
        public virtual ICollection<CourseAssignment> Assignments { get; set; }

        public virtual Department Departmemt { get; set; }



        // Calculated Property 
        // Return the CourseID and CourseTitle

        public string CourseIDTitle
        {
            get
            {
                return CourseID + ": " + Title;  // ie.   1: Math  2: English
            }
        }



    }
}