using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace API
{
    public class Validations
    {
        public static bool ValidateNumberofBooks(int numberofBooks)
        {
            if (numberofBooks >= 5) return false;
            if (numberofBooks <= 0) return false;
            return true;
        }
        public static bool ValidateUserName(string xUserName)
        {
            Regex name = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
            if(name.IsMatch(xUserName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ValidatePassword(string xPassword)
        {
            Regex password = new Regex("^[a-zA-Z][a-zA-Z0-9@!#,.]*$");
            if (password.IsMatch(xPassword))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}