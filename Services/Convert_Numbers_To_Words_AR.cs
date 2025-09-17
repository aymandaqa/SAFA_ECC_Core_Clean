using System;
using System.Globalization;

namespace SAFA_ECC_Core_Clean.Services
{
    public class Convert_Numbers_To_Words_AR
    {
        private static string[] Ones = new string[] { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };
        private static string[] Tens = new string[] { "", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
        private static string[] Teens = new string[] { "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
        private static string[] Hundreds = new string[] { "", "مائة", "مئتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };
        private static string[] Thousands = new string[] { "", "ألف", "ألفان", "ثلاثة آلاف", "أربعة آلاف", "خمسة آلاف", "ستة آلاف", "سبعة آلاف", "ثمانية آلاف", "تسعة آلاف" };
        private static string[] Millions = new string[] { "", "مليون", "مليونان", "ملايين" };
        private static string[] Billions = new string[] { "", "مليار", "ملياران", "مليارات" };

        public string ConvertNumberToWords(decimal number, string currency)
        {
            string result = "";
            string integerPart = Math.Floor(number).ToString();
            string decimalPart = ((int)((number - Math.Floor(number)) * 100)).ToString();

            result += ConvertIntegerToWords(long.Parse(integerPart));
            result += " " + GetCurrencyName(currency, long.Parse(integerPart));

            if (decimalPart != "0" && decimalPart != "00")
            {
                result += " و" + ConvertIntegerToWords(long.Parse(decimalPart));
                result += " " + GetDecimalCurrencyName(currency, long.Parse(decimalPart));
            }

            return result.Trim();
        }

        private string ConvertIntegerToWords(long number)
        {
            if (number == 0) return "صفر";

            string words = "";

            if (number < 10) words = Ones[number];
            else if (number < 20) words = Teens[number - 10];
            else if (number < 100) words = Tens[number / 10] + (number % 10 > 0 ? " و" + Ones[number % 10] : "");
            else if (number < 1000) words = Hundreds[number / 100] + (number % 100 > 0 ? " و" + ConvertIntegerToWords(number % 100) : "");
            else if (number < 1000000)
            {
                long thousands = number / 1000;
                long remainder = number % 1000;
                words = ConvertIntegerToWords(thousands) + " " + GetThousandsName(thousands) + (remainder > 0 ? " و" + ConvertIntegerToWords(remainder) : "");
            }
            else if (number < 1000000000)
            {
                long millions = number / 1000000;
                long remainder = number % 1000000;
                words = ConvertIntegerToWords(millions) + " " + GetMillionsName(millions) + (remainder > 0 ? " و" + ConvertIntegerToWords(remainder) : "");
            }
            else
            {
                long billions = number / 1000000000;
                long remainder = number % 1000000000;
                words = ConvertIntegerToWords(billions) + " " + GetBillionsName(billions) + (remainder > 0 ? " و" + ConvertIntegerToWords(remainder) : "");
            }

            return words;
        }

        private string GetThousandsName(long number)
        {
            if (number == 1) return "ألف";
            if (number == 2) return "ألفان";
            if (number >= 3 && number <= 10) return "آلاف";
            return "ألف"; // Default for other cases
        }

        private string GetMillionsName(long number)
        {
            if (number == 1) return "مليون";
            if (number == 2) return "مليونان";
            if (number >= 3 && number <= 10) return "ملايين";
            return "مليون"; // Default for other cases
        }

        private string GetBillionsName(long number)
        {
            if (number == 1) return "مليار";
            if (number == 2) return "ملياران";
            if (number >= 3 && number <= 10) return "مليارات";
            return "مليار"; // Default for other cases
        }

        private string GetCurrencyName(string currency, long amount)
        {
            switch (currency.ToUpper())
            {
                case "JOD": return amount == 1 ? "دينار أردني" : amount == 2 ? "ديناران أردنيان" : (amount >= 3 && amount <= 10) ? "دنانير أردنية" : "دينار أردني";
                case "USD": return amount == 1 ? "دولار أمريكي" : amount == 2 ? "دولاران أمريكيان" : (amount >= 3 && amount <= 10) ? "دولارات أمريكية" : "دولار أمريكي";
                case "ILS": return amount == 1 ? "شيكل إسرائيلي" : amount == 2 ? "شيكلان إسرائيليان" : (amount >= 3 && amount <= 10) ? "شيكلات إسرائيلية" : "شيكل إسرائيلي";
                case "EUR": return amount == 1 ? "يورو" : amount == 2 ? "يورو" : (amount >= 3 && amount <= 10) ? "يورو" : "يورو";
                default: return currency;
            }
        }

        private string GetDecimalCurrencyName(string currency, long amount)
        {
            switch (currency.ToUpper())
            {
                case "JOD": return amount == 1 ? "فلس" : amount == 2 ? "فلسان" : (amount >= 3 && amount <= 10) ? "فلوس" : "فلس";
                case "USD": return amount == 1 ? "سنت" : amount == 2 ? "سنتان" : (amount >= 3 && amount <= 10) ? "سنتات" : "سنت";
                case "ILS": return amount == 1 ? "أجورا" : amount == 2 ? "أجورات" : (amount >= 3 && amount <= 10) ? "أجورات" : "أجورا";
                case "EUR": return amount == 1 ? "سنت" : amount == 2 ? "سنتان" : (amount >= 3 && amount <= 10) ? "سنتات" : "سنت";
                default: return "";
            }
        }
    }
}

