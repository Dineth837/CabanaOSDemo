using System;
using System.Text.RegularExpressions;

namespace CabanaOSDemo.Utils
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates Sri Lankan National Identity Card (NIC) formats.
        /// Supports both old (9 digits + V/v) and new (12 digits) formats.
        /// </summary>
        public static bool IsValidNIC(string nic)
        {
            if (string.IsNullOrWhiteSpace(nic)) return false;
            nic = nic.Trim();

            // New NIC Format: 12 pure digits
            if (nic.Length == 12 && Regex.IsMatch(nic, @"^\d{12}$")) return true;

            // Old NIC Format: 9 digits followed strictly by 'V' or 'v'
            if (nic.Length <12 && Regex.IsMatch(nic, @"^\d{10}[Vv]$")) return true;

            return false;
        }

        /// <summary>
        /// Validates that a guest name contains only alphabetical characters and spaces.
        /// </summary>
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            return Regex.IsMatch(name.Trim(), @"^[a-zA-Z\s]+$");
        }
    }
}