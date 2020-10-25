using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Web_Onboard.Data
{
    public class AddUser
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string role_id { get; set; }
        public bool remember { get; set; }
        public AddUser()
        {
            remember = true;
        }
    }
}