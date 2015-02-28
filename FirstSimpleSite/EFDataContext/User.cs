using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstSimpleSite.EFDataContext
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
    }
}