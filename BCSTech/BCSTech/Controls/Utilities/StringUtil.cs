using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace BCSTech.Controls.Utilities
{
    public class StringUtil
    {
        internal static JObject ToJObject(string str)
        {
            try
            {
                str = str.Insert(0, "{ \"obj\": ");
                str += "}";
                return JObject.Parse(str);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        static Regex ValidEmailRegex = CreateValidEmailRegex();
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        internal static bool EmailIsValid(string emailAddress) => ValidEmailRegex.IsMatch(emailAddress);

        internal static bool EmailNetIsValid(string emailAddress)
        {
            bool validity = false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(emailAddress);
                validity = addr.Address == emailAddress;
            }
            catch
            {
                validity = false;
            }
            return validity;
        }
    }
}
