using System.Collections;
using System.Collections.Generic;
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

        public int CourseID { get; set; } //PK

        public string Title { get; set; }

        public int Credits { get; set; }

        // ---------- Navigation Properties ---------- //
        // 1 course, many enrollments 
        public virtual ICollection<Enrollment> Enrollments { get; set; }



    }
}