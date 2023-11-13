using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Productos.Models
{
    public class Usuario
    {
        public int id { get; set; }

        public string username { get; set; }

        public string email { get; set; }

        public string password { get; set; }


        public string cpassword { get; set; }



    }
}