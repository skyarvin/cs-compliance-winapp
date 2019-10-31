using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkydevCSTool.Class
{
    public static class Url
    {
        //##################SET TO FALSE WHEN DEPLOYING ON PRODUCTION #########################//
        public static bool DEBUG = true;
        //####################################################################################//



        // ## Production
        public static string CB_HOME = "https://chaturbate.com/";
        public static string CB_COMPLIANCE_URL = "https://chaturbate.com/compliance";
        public static string CB_COMPLIANCE_SET_ID_EXP_URL = "https://chaturbate.com/compliance/update_expiration_date_form";
        public static string GOOGLE_TRANSLATE_URL = "https://translate.google.com?text=";
        public static string API_URL = "https://cscb.skydev.solutions/api";

        // ## Local Testing with Live Rooms
        //public static string CB_HOME = "https://chaturbate.com/";
        //public static string CB_COMPLIANCE_URL = "https://chaturbate.com/compliance";
        //public static string CB_COMPLIANCE_SET_ID_EXP_URL = "https://chaturbate.com/compliance/update_expiration_date_form";
        //public static string GOOGLE_TRANSLATE_URL = "https://translate.google.com?text=";
        //public static string API_URL = "https://cscb-dev.skydev.solutions/api";

        // ## Pure Local Testing with MockSite
        //public static string CB_HOME = "http://10.10.10.239:8080/cb";
        //public static string CB_COMPLIANCE_URL = "http://10.10.10.239:8080/cb";
        //public static string CB_COMPLIANCE_SET_ID_EXP_URL = "https://chaturbate.com/compliance/update_expiration_date_form";
        //public static string GOOGLE_TRANSLATE_URL = "https://translate.google.com?text=";
        //public static string API_URL = "https://cscb-dev.skydev.solutions/api";
    }
}
