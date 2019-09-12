using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkydevCSTool.Models
{
    public class UserAccount
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        /// <summary>
        /// API get to fetch account from  User pool
        /// </summary>
        /// <returns></returns>
        public static UserAccount GetAccount()
        {
            return new UserAccount()
            {
                id = 0,
                username = "skychrisp",
                password = "FBBAFyAw%[r{)5z?"
            };
        }
    }
}
