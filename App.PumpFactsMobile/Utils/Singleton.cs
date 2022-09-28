using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.PumpFactsMobile.Utils
{
    public static class Singleton
    {
        public static string getServiceAddress()
        {
            string profile;
#if (DEBUG)
            profile = "http://192.168.51.196:5000";
#else
            profile = "http://xxxxxxx";
#endif
            return profile;
        }
    }
}
