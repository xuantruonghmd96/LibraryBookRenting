using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Installers
{
    public class JwtSettings
    {
        public string Secret { get; set; }
    }

    public class BussinessConfiguration
    {
        public int BookQuantityInStock { get; set; }
    }

    public static class MyAppSettings
    {
        public static BussinessConfiguration BussinessConfiguration { get; set; } = new BussinessConfiguration();
        public static JwtSettings JwtSettings { get; set; } = new JwtSettings();
    }
}
