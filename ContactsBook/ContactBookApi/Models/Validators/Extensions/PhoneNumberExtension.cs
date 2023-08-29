using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContactsBook.Models.Validators.Extensions
{
    public static class PhoneNumberExtension
    {
        public static bool IsNumberCorrect(this string phoneNumber)
        {
            return Regex.Match(phoneNumber, @"^(\+[0-9])$").Success;
        }
    }
}
