using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartQA.Models
{
    public class UserViewModels
    {
        public string ID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Username:")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Display(Name = "Phone:")]
        public string Phone { get; set; }

        [Display(Name = "Picture:")]
        public string Picture { get; set; }

        [Display(Name = "Company:")]
        public string Company { get; set; }

        [Display(Name = "Time Zone:")]
        public int TimeZone { get; set; }
    }
}