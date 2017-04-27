namespace ContosoU2016.Models
{
    // Grade Enumeration
    public enum Grade
    {
        A,B,C,D,F
    }


    public class Enrollment
    {
        public int EnrollmentID { get; set; } // PK
        //The CourseID property is a foreign key and the corresponding navigation property is Course. 
        //An enrollment Entity is associated with one Course Entity. 
        
        public int CourseID { get; set; } // FK
        public int StudentID { get; set; } // FK
        // The StudentID property is a foreign key and the corresponding navigation property is Student. 
        // An enrollment Entity is associated with one Studnet Entity, so the property can only hold a single Student Entity.

        // Entity Framework interprets a property as a foreign key property if it's named <nagigation property name><primary key property name> 
        // for example: 
        // StudentID for the Student Navigation property, since the Student entity's primary ket is ID 
        // (Inherits from Person Entity ID Property in this case) 

        // Foreign key properties can also be named simple <primary key property name> for example: 
        // CourseID, since the Course Entity's primary key is Course ID. 

        public Grade? Grade { get; set; }  // ? = Nullable : because students aren't given a grade when registering. 

        // ---------- Navigation Properties ---------- //
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }

    }
}