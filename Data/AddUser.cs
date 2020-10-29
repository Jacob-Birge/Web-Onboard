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
        public string role_id { get; set; }
        public string email { get; set; }

        public AddUser()
        {
            username = "";
            role_id = "";
            email = "";
        }
    }
}