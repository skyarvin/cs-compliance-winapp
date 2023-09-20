using CSTool.Handlers;
using CSTool.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Models
{
    internal class UserToken
    {
        private EncryptionHandler encryptionHandler;

        public UserToken()
        {
            this.encryptionHandler = new EncryptionHandler();
        }

        public string access_token {
            get => Settings.Default.access_token;
            set => Settings.Default.access_token = value;
        }
        public string refresh_token {
            get => Settings.Default.refresh_token;
            set => Settings.Default.refresh_token = value;
        }
    }
}
