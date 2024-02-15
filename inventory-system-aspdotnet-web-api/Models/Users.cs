using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inventory_system_aspdotnet_web_api.Models
{
    public class Users
    {


        [Required(ErrorMessage = "Please Enter Name...")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Password...")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "Please Enter Email...")]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }

    public class LoginUsers : Users
    {
        private new string Name { get; set; }
    }


    public class GetUsers : Users
    {

        [Key]
        public int UserId { get; set; }
        private new string Password { get; set; }


    }

    public class ReturnLoginUsers
    {

        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }



}