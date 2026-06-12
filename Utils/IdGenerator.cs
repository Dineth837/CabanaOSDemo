using System;

namespace CabanaOSDemo.Utils
{
    public static class IdGenerator
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Extracts a fallback segment from the NIC token to use inside the generated reference ID string.
        /// </summary>
        private static string GetNicSegment(string nic)
        {
            if (string.IsNullOrEmpty(nic) || nic.Length < 4) return "GUST";
            return nic.Trim().Substring(0, 4).ToUpper();
        }

        /// <summary>
        /// Generates a unique transaction tracking code for Suite Accommodations.
        /// Examples: STF1024GUST, SUP85922004
        /// </summary>
        public static string GenerateSuiteBookingID(string tier, string type, string customerNic)
        {
            int uniqueNum = _random.Next(1000, 9999);
            string nicPart = GetNicSegment(customerNic);

            if (tier.Equals("Standard", StringComparison.OrdinalIgnoreCase))
            {
                return type.Equals("Family", StringComparison.OrdinalIgnoreCase)
                    ? $"STF{uniqueNum}{nicPart}"
                    : $"STC{uniqueNum}{nicPart}";
            }

            return tier.Equals("Superior", StringComparison.OrdinalIgnoreCase)
                ? $"SUP{uniqueNum}{nicPart}"
                : $"DLX{uniqueNum}{nicPart}";
        }

        /// <summary>
        /// Generates a unique tracking code for Restaurant Table Reservations.
        /// Examples: STCZ4829GUST, DLXFZ93011994
        /// </summary>
        public static string GenerateRestaurantReservationID(string zone, string type, string leadNic)
        {
            int uniqueNum = _random.Next(1000, 9999);
            string nicPart = GetNicSegment(leadNic);
            bool isCouple = type.Contains("Couple", StringComparison.OrdinalIgnoreCase);

            if (zone.Equals("Standard", StringComparison.OrdinalIgnoreCase))
            {
                return isCouple ? $"STCZ{uniqueNum}{nicPart}" : $"STFZ{uniqueNum}{nicPart}";
            }

            return isCouple ? $"DLXCZ{uniqueNum}{nicPart}" : $"DLXFZ{uniqueNum}{nicPart}";
        }

        public static string GenerateCustomerID(string nic)
        {
            string numericPart;

            // 1. Normalize NIC to get only numeric digits
            if (nic.EndsWith("V", StringComparison.OrdinalIgnoreCase) || nic.EndsWith("X", StringComparison.OrdinalIgnoreCase))
            {
                // Handle old format: 748490001V -> 748490001
                numericPart = nic.Substring(0, 9);
            }
            else
            {
                // Handle modern format: 200421701650 (Keep all 12)
                numericPart = nic;
            }

            // 2. Pad or split to ensure we have enough digits for your formula
            // For old format, we pad to match the 12-digit length logic
            string paddedNic = numericPart.PadRight(12, '0');

            try
            {
                int part1 = int.Parse(paddedNic.Substring(0, 4));
                int part2 = int.Parse(paddedNic.Substring(4, 4));
                int part3 = int.Parse(paddedNic.Substring(8, 4));

                // Apply your formula: (((part2 + part3) / 2) + part1) / 6
                double result = (((part2 + part3) / 2.0) + part1) / 6.0;

                return $"CuS{Math.Round(result)}";
            }
            catch
            {
                return "CuS0000";
            }
        }


        public static string GenerateRevID()
        {
            // 1. Get the exact current moment on the device
            DateTime now = DateTime.Now;

            // 2. Get the 3-letter month abbreviation (e.g., "JUN")
            string monthAbbr = now.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture).ToUpper();

            // 3. Get the last two digits of the year (e.g., "26")
            string yearShort = now.Year.ToString().Substring(now.Year.ToString().Length - 2);

            // 4. Combine them (Result: "JUN26")
            return $"{monthAbbr}{yearShort}";
        }
    }
}