using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace KingCyber.Base
{
    public class GameUtils 
    {
        public static string ConvertMoneyDotFormatted(long money, string dot = ",")
        {
            var moneyStr = money.ToString();
            var reversedStr = new string(moneyStr.Reverse().ToArray());
            var result = string.Join(dot, Enumerable.Range(0, reversedStr.Length / 3 + (reversedStr.Length % 3 == 0 ? 0 : 1))
                                                     .Select(i => reversedStr.Substring(i * 3, Math.Min(3, reversedStr.Length - i * 3))));
            return new string(result.Reverse().ToArray());
        }

        public static string ConvertMoneyShortFormatted(long money)
        {
            if (money >= 1_000_000_000)
            {
                return (money / 1_000_000_000D).ToString("0.##") + "B";
            }
            else if (money >= 1_000_000)
            {
                return (money / 1_000_000D).ToString("0.##") + "M";
            }
            else if (money >= 100_000)
            {
                return (money / 1_000D).ToString("0.##") + "K";
            }
            else
            {
                return ConvertMoneyDotFormatted(money);
            }
        }

        public static string ConvertRemainTimeMMSS(float remainTime)
        {
            //int hours = Mathf.FloorToInt(remainTime / 3600);
            int minutes = Mathf.FloorToInt((remainTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(remainTime % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }

        public static string ConvertRemainTimeHHMMSS(float remainTime)
        {
            int hours = Mathf.FloorToInt(remainTime / 3600);
            int minutes = Mathf.FloorToInt((remainTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(remainTime % 60);
            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }

        public static void GetCountryName(out string countryCode, out string countryName)
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            RegionInfo regionInfo = new RegionInfo(currentCulture.Name);
            countryCode = regionInfo.TwoLetterISORegionName; // ISO 3166-1 alpha-2 code (e.g., "US", "VN")
            countryName = regionInfo.EnglishName; // Full country name in English (e.g., "United States", "Vietnam")
            Debug.Log("Device Country Code: " + countryCode);
            Debug.Log("Device Country Name: " + countryName);
        }
    }
}

