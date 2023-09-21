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
        public static string KNOWLEDGE_BASE_URL = "https://staffme.online/knowledge-base/";
        //####################################################################################//

        // ## Production CSCB-Staff Me
        public static string CB_HOME = "https://chaturbate.com/";
        public static string CB_COMPLIANCE_URL = "https://chaturbate.com/compliance";
        public static string CB_COMPLIANCE_SET_ID_EXP_URL = "https://chaturbate.com/compliance/update_expiration_date_form";
        public static string GOOGLE_TRANSLATE_URL = "https://translate.google.com?text=";
        public static string API_URL = "https://cscb.staffme.online/api";
        public static string AUTH_URL = "https://cscb.staffme.online/chat_auth";
        public static string DOMAIN = "chaturbate.com";

        // ## Production Volume
        //public static string CB_HOME = "https://volume.com/";
        //public static string CB_COMPLIANCE_URL = "https://volume.com/compliance/";
        //public static string CB_COMPLIANCE_SET_ID_EXP_URL = "https://volume.com/compliance/update_expiration_date_form";
        //public static string GOOGLE_TRANSLATE_URL = "https://translate.google.com?text=";
        //public static string API_URL = "https://csvolume.skydev.solutions/api";
        //public static string DOMAIN = "volume.com";

        // ## Local Testing with Live Rooms
        //public static string CB_HOME = "https://chaturbate.com/";
        //public static string CB_COMPLIANCE_URL = "https://chaturbate.com/compliance";
        //public static string CB_COMPLIANCE_SET_ID_EXP_URL = "https://chaturbate.com/compliance/update_expiration_date_form";
        //public static string GOOGLE_TRANSLATE_URL = "https://translate.google.com?text=";
        //public static string API_URL = "https://cscb.staffme.online/api";

        // ## Pure Local Testing with MockSite
        //public static string CB_HOME = "http://127.0.0.1:8000/compliance/show/razer";
        //public static string DOMAIN = "http://127.0.0.1:8000/compliance";
        //public static string CB_COMPLIANCE_URL = "http://127.0.0.1:8000/compliance";
        //public static string CB_COMPLIANCE_SET_ID_EXP_URL = "http://127.0.0.1:8000/compliance/update_expiration_date_form";
        //public static string GOOGLE_TRANSLATE_URL = "https://translate.google.com?text=";
        //public static string API_URL = "http://127.0.0.1:9000/api";
        //public static string AUTH_URL = "http://127.0.0.1:9000/chat_auth";
    }
}