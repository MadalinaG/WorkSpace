using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartQA.Models
{
    public class ContactModels
    {
        public int ID { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Adress")]
        public string Email { get; set; }

        [Required]
        [StringLength(4000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public DateTime AddedTime { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string Message { get; set; }


        public int Subject { get; set; }
    }
}