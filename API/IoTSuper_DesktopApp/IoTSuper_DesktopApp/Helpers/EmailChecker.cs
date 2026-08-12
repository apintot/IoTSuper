using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace IoTSuper_DesktopApp.Helpers
{
    public static class EmailChecker
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
