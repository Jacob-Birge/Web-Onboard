using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Onboard.Data
{
    public class User
    {
        public string username;
        public string password;
        public bool remember;
        public User()
        {
            remember = true;
        }
    }
}
