using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Class
{
    public static class Url
    {
        //##################SET TO FALSE WHEN DEPLOYING ON PRODUCTION #########################//
        public static bool DEBUG = false;
        public static bool DEV_API = false;
        public static string KNOWLEDGE_BASE_URL = "https://staffme.online/knowledge-base/";
        //####################################################################################//
        

        // ## Production CSCB-Staff Me
        public static string CB_HOME = "https://chaturbate.com/";
        public static string CB_COMPLIANCE_URL = CB_HOME + "compliance";
        public static string CB_COMPLIANCE_SET_ID_EXP_URL = CB_HOME + "compliance/update_expiration_date_form";
        public static string GOOGLE_TRANSLATE_URL = "https://translate.google.com?text=";
        public static string BASE_URL = "https://cscb.staffme.online";
        public static string API_URL = BASE_URL + "/api";
        public static string AUTH_URL = BASE_URL + "/security";
        public static string DOMAIN = "chaturbate.com";

        // ## Production Volume
        //public static string CB_HOME = "https://volume.com/";
        //public static string CB_COMPLIANCE_URL = CB_HOME + "compliance";
        //public static string CB_COMPLIANCE_SET_ID_EXP_URL = CB_HOME + "compliance/update_expiration_date_form";
        //public static string GOOGLE_TRANSLATE_URL = "https://translate.google.com?text=";
        //public static string API_URL = "https://csvolume.skydev.solutions/api";
        //public static string DOMAIN = "volume.com";

        static Url()
        {
            if (DEBUG && !DEV_API)
            {
                // ## Pure Local Testing with MockSite
                CB_HOME = "http://127.0.0.1:8000/";
                BASE_URL = "http://127.0.0.1:9000";
                DOMAIN = "http://127.0.0.1:8000/compliance";
            }
            else if (DEBUG && DEV_API)
            {
                // ## Dev Testing with MockSite
                CB_HOME = "http://127.0.0.1:8000/";
                BASE_URL = "https://cscb-dev1.staffme.online";
                DOMAIN = "http://127.0.0.1:8000/compliance";
            }
            else if (!DEBUG && DEV_API)
            {
                // ## Dev Testing with Live Rooms
                BASE_URL = "https://cscb-dev1.staffme.online";
            }

            CB_COMPLIANCE_URL = CB_HOME + "compliance";
            CB_COMPLIANCE_SET_ID_EXP_URL = CB_HOME + "compliance/update_expiration_date_form";
            API_URL = BASE_URL + "/api";
            AUTH_URL = BASE_URL + "/security";
        }
    }
}