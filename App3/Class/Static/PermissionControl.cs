using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class.Static
{
    static class PermissionControl
    {
        private const int ACCEPTED_USER = 37;
        private static int current_user = 0;
        private static string user_name = "";
        public static string UserName { get { return user_name; } }

        public static bool auth(string pass = "")
        {
            if (pass != "")
            {
                object[] o = DataBase.FirstRow(string.Format("SELECT \"user\" FROM users WHERE pass = md5('{0}')" , pass), 0);
                if (o.Length > 0)
                {
                    current_user = ACCEPTED_USER;
                    user_name = o[0].ToString();
                }
            }
            if (current_user == ACCEPTED_USER)
                return true;
            else
                return false;
        }

        public static void logout()
        {
            current_user = 0;
            user_name = "";
        }

    }
}
