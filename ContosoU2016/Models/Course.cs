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
        public string Title { get; set; }

        [Range(0,5)]
        public int Credits { get; set; }

        // ---------- Navigation Properties ---------- //
        // 1 course, many enrollments 
        public virtual ICollection<Enrollment> Enrollments { get; set; }



    }
}