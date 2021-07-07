using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Mvc;

namespace TeliaMVC.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Admin
    {
        public int Id { get; set; }
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "This field is Required")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is Required")]
        public string Password { get; set; }

        public string LoginErrorMsg { get; set; }
    }
}
