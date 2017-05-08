using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models
{
    public abstract class Person // abstract to allow inheirtance 
    {
        // lwilliston:  Create the data models.

        public int ID { get; set; }

        [Required]
        [StringLength(65, ErrorMessage = "Last Name cannot be longer than 65 Characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 Characters")]
        [Display(Name = "First Name")]
        public string  FirstName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // FullName:  Calculated property with a get accessor only
        //            Will not get generated in database. 

        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public string IdFullName
        {
            get
            {
                return "(" + ID + ") " + LastName + "," + FirstName;
            }
        }
    }
}
