using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.ViewModels
{
    public class PatientCreateViewModel
    {
        // Primary Key - Unique ID for each patient
        public int PatientId { get; set; }

        // Required means this field cannot be empty
        // ErrorMessage shows custom message if validation fails


        [Required(ErrorMessage = "Patient name is required")]


        public string PatientName { get; set; }

        // DataType(Date) tells MVC to show date picker
        // Display changes how label appears in form
        [DataType(DataType.Date)]


        public DateTime? Dob { get; set; }

        // Required means user must select gender

        [Required(ErrorMessage = "Gender is required")]
        //  [Display(Name = "Gender")]
        public string Gender { get; set; }

        // Required → cannot be empty
        // Phone → checks if entered value is valid phone format

        [Required(ErrorMessage = "Contact number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact must be 10 digits")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numbers allowed")]

        public string Contact { get; set; }

        // EmailAddress → validates correct email format
        // This field is optional (no Required attribute)
        [EmailAddress(ErrorMessage = "Invalid email address")]
        // [Display(Name = "Email Address")]
        public string Email { get; set; }




        // required filed cannot be empty
        [Required(ErrorMessage = "Address is requireed")]
        public string Address { get; set; }

        // This is auto-generated in database (GETDATE())
        // Nullable because it may not be set manually
        public DateTime? CreatedAt { get; set; }

    }
}
