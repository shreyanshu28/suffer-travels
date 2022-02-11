﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    public class User
    {
        [Key]
        public UInt32 UId { get; set; }
        [Required(ErrorMessage = "First name is required!")]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "First name should only contain alphabets!")]
        public string Fname { get; set; }
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Middle name should only contain alphabets!")]
        public string? Mname { get; set; } = "";
        [Required(ErrorMessage = "Last name is required!")]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Last name should only contain alphabets!")]
        public string Lname { get; set; }
        [Required(ErrorMessage = "Gender is required!")]
        public char Gender { get; set; } = 'M';
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Contact number is required!")]
        [RegularExpression("^[7-9][0-9]{10}", ErrorMessage = "Please enter valid contact number")]
        //[DataType(DataType.PhoneNumber, ErrorMessage = "Please enter valid contact number")]
        [MaxLength(10, ErrorMessage = "Please enter valid contact number")]
        [MinLength(10, ErrorMessage = "Please enter valid contact number")]
        public long ContactNo { get; set; }
        [Required(ErrorMessage = "Email address is required!")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required!")]
        [StringLength(50, ErrorMessage = "Username should have 3 to 50 characters only", MinimumLength = 3)]
        [MinLength(3, ErrorMessage = "Username must be 3-50")]
        [RegularExpression("^[a-zA-Z][0-9a-zA-Z]{3, 50}", ErrorMessage = "Username is not valid!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        [StringLength(20, ErrorMessage = "Password minimum 8 character", MinimumLength = 8)]
        public string Password { get; set; }
        public string ProfilePhoto { get; set; } = "";
        public Boolean IsActive { get; set; } = true;
        [ForeignKey("Role")]
        public UInt32 RoleId { get; set; }

    }
}
