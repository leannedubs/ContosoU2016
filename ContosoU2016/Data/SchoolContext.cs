using ContosoU2016.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Data
{
    // Create the SchoolContext  -- databse context for our University database.

    public class SchoolContext : DbContext  // Derives from the System.Data.Entity.DbContext class
    {
        // Constructor 
        public SchoolContext(DbContextOptions<SchoolContext>options):base(options) 
        {

        }

        // Specifying Entity Sets  - corresponding to databse tables and each single 
        // entity corresponds to a row in a table 
        public DbSet<Person> People { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        // Complex Data Model
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }


        // When the database is created, EF creates tables that have names the same as the DBSet property names. 
        // Property Names for collections are typically plural (Studnets rather than Student)
        // Developers disagree about whether table names should be pluralized or not. 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<Person>().ToTable("People");

            // Complex Data Model
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");
            modelBuilder.Entity<Department>().ToTable("Department");

            // Composite PK on CourseAssignment.
            modelBuilder.Entity<CourseAssignment>().HasKey(c => new { c.CourseID, c.InstructorID });
        }
    }
}
