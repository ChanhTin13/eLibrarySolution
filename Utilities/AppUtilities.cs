﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public static class AppUtilities
    {
        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ",};

            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
            "d",
            "e","e","e","e","e","e","e","e","e","e","e",
            "i","i","i","i","i",
            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
            "u","u","u","u","u","u","u","u","u","u","u",
            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        public static bool CheckUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ",};
            bool check = false;
            for (int i = 0; i < arr1.Length; i++)
            {
                if (text.Contains(arr1[i]))
                    check = true;
            }
            return check;
        }

        /// <summary>
        /// Lấy web Path Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetPathFromUrl(string url)
        {
            return url.Split('?')[0].Split('#')[0];
        }
        /// <summary>
        /// kiểu giờ học HH:mm
        /// </summary>
        /// <returns></returns>
        public static bool ValidateStudyTime(string time)
        {
            var times = time.Split(":");
            if (times.Length != 2)
                return false;
            int i;
            if (!int.TryParse(times[0], out i))
                return false;
            if (i > 24)
                return false;
            if (!int.TryParse(times[1], out i))
                return false;
            if (i >= 60)
                return false;
            return true;
        }
        public static string ConvertStutyTimeByTimeZone(string time, double timezone)
        {
            if(!ValidateStudyTime(time))
                return "";
            var times = time.Split(":");
            if (times.Length != 2)
                return "";
            int i;
            if (!int.TryParse(times[0], out i))
                return "";
            if (i > 24)
                return "";
            int i2;
            if (!int.TryParse(times[1], out i2))
                return "";
            if (i2 >= 60)
                return "";
            DateTime date = new DateTime(1970, 1, 1, i, i2, 0);
            date = date.AddHours(timezone);
            return date.Hour + ":" + date.Minute;
        }
        public static string ConvertStutyTimeToUTC(string time, double timezone)
        {
            if (!ValidateStudyTime(time))
                return "";
            var times = time.Split(":");
            if (times.Length != 2)
                return "";
            int i;
            if (!int.TryParse(times[0], out i))
                return "";
            if (i > 24)
                return "";
            int i2;
            if (!int.TryParse(times[1], out i2))
                return "";
            if (i2 >= 60)
                return "";
            DateTime date = new DateTime(1970, 1, 1, i, i2, 0);
            date = date.AddHours(-timezone);
            return date.Hour + ":" + date.Minute;
        }
    }
}
