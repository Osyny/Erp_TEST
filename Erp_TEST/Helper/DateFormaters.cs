using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Helper.DateFormaters
{
    public enum DateParseTypes
    {
        Document,
        Birthday,
        Register,
        /// <summary>
        /// Для свершившихся событий, например времени задержания.
        /// </summary>
        FulfilledEvent,
        /// <summary>
        /// Для поисков - они должны учитывать дату "до" включительно
        /// </summary>
        DateToForSearch,
        InFuture
    }
    public class DateParsers
    {
        /// <summary>
        /// Строка в формате "dd.MM.yyyy"
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DateParseResult ddMMyyyy(string dateString, DateParseTypes type)
        {
            var res = ddMMyyyy(dateString);
            if (!res.IsValid)
            {
                return res;
            }

            if (type == DateParseTypes.DateToForSearch)
            {
                res.Value = res.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
            }

            switch (type)
            {
                case DateParseTypes.Birthday:
                case DateParseTypes.Document:
                case DateParseTypes.Register:
                case DateParseTypes.FulfilledEvent:
                    //if (res.Value < DateTime.Now)
                    if (res.Value > DateTime.Now)
                    {
                        res.IsValid = false;
                        res.ErrorMessage = $"Дата не може бути пізніше сьогоднішньої";
                    }
                    break;
                case DateParseTypes.InFuture:
                    if (res.Value <= DateTime.Now)
                    {
                        res.IsValid = false;
                        res.ErrorMessage = $"Дата не може бути раніше сьогоднішньої";
                    }
                    break;
            }
            return res;
        }

        public static CombinedDateParseResult ddMMyyyyHHmm(string dateString, string timeString, DateParseTypes type)
        {
            var dateRes = ddMMyyyy(dateString);
            var timeRes = HHmm(timeString);
            CombinedDateParseResult res = new CombinedDateParseResult() { DateResult = dateRes, TimeResult = timeRes };
            if (!dateRes.IsValid || !timeRes.IsValid)
            {
                return res;
            }


            DateTime combined = new DateTime(dateRes.Value.Year, dateRes.Value.Month, dateRes.Value.Day,
                timeRes.Value.Hour, timeRes.Value.Minute, 0);

            res.Value = combined;
            res.IsValid = true;

            switch (type)
            {
                case DateParseTypes.Birthday:
                case DateParseTypes.Document:
                case DateParseTypes.Register:
                case DateParseTypes.FulfilledEvent:
                    if (combined > DateTime.Now)
                    {
                        res.TimeResult.IsValid = false;
                        res.DateResult.IsValid = false;
                        res.IsValid = false;
                        res.TimeResult.ErrorMessage = $"Дата не може бути пізніше сьогоднішньої";
                        res.DateResult.ErrorMessage = $"Дата не може бути пізніше сьогоднішньої";
                    }
                    break;
            }
            return res;
        }

        public static DateParseResult ddMMyyyy(string dateString)
        {
            if (dateString == null)
            {
                dateString = string.Empty;
            }

            dateString = dateString.Replace("-", ".");

            bool isValid = DateTime.TryParseExact(dateString, "yyyy.MM.dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt);


            if (dt.Year == 0001 || dt.Year == 1 || dt.Year < 1900 || dt.Year > DateTime.Now.Year + 50)
            {
                isValid = false;
            }

            return new DateParseResult()
            {
                Value = dt,
                IsValid = isValid
            };
        }

        // ----------------------------------------------
        /// <summary>
        /// Строка в формате "dd.MM.yyyy"
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DateParseResult ddMMyyyyHHmm(string dateString, DateParseTypes type)
        {
            var res = ddMMyyyyHHmm(dateString);
            if (!res.IsValid)
            {
                return res;
            }

            switch (type)
            {
                case DateParseTypes.Birthday:
                case DateParseTypes.Document:
                case DateParseTypes.Register:
                    if (res.Value > DateTime.Now)
                    {
                        res.IsValid = false;
                        res.ErrorMessage = $"Дата не може бути пізніше сьогоднішньої";
                    }
                    break;
            }
            return res;
        }

        /// <summary>
        /// Строка в формате "dd.MM.yyyy HH:mm"
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static DateParseResult ddMMyyyyHHmm(string dateString)
        {
            if (dateString == null)
            {
                dateString = string.Empty;
            }

            bool isValid = DateTime.TryParseExact(dateString, "dd.MM.yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt);


            if (dt.Year == 0001 || dt.Year == 1 || dt.Year < 1900 || dt.Year > DateTime.Now.Year + 50)
            {
                isValid = false;
            }

            return new DateParseResult()
            {
                Value = dt,
                IsValid = isValid
            };
        }

        /// <summary>
        /// Строка в формате "dd.MM.yyyy HH-mm"
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static DateParseResult ddMMyyyyHHmmFile(string dateString)
        {
            if (dateString == null)
            {
                dateString = string.Empty;
            }

            bool isValid = DateTime.TryParseExact(dateString, "dd.MM.yyyy HH-mm",
              CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt);


            if (dt.Year == 0001 || dt.Year == 1 || dt.Year < 1900 || dt.Year > DateTime.Now.Year + 50)
            {
                isValid = false;
            }

            return new DateParseResult()
            {
                Value = dt,
                IsValid = isValid
            };
        }

        /// <summary>
        /// Строка в формате "HHmm"
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DateParseResult HHmm(string dateString, DateParseTypes type)
        {
            var res = HHmm(dateString);
            if (!res.IsValid)
            {
                return res;
            }

            switch (type)
            {
                case DateParseTypes.Birthday:
                    if (res.Value > DateTime.Now)
                    {
                        res.IsValid = false;
                        res.ErrorMessage = $"Дата не може бути пізніше сьогоднішньої";
                    }
                    break;
                case DateParseTypes.Document:
                case DateParseTypes.Register:
                    if (res.Value > DateTime.Now)
                    {
                        res.IsValid = false;
                        res.ErrorMessage = $"Дата не може бути пізніше сьогоднішньої";
                    }
                    break;
            }
            return res;
        }

        /// <summary>
        /// Строка в формате "HH:mm"
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static DateParseResult HHmm(string dateString)
        {
            if (dateString == null)
            {
                dateString = string.Empty;
            }

            bool isValid = DateTime.TryParseExact(dateString, "HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt);


            //if (dt.Year == 0001 || dt.Year == 1 || dt.Year < 1900 || dt.Year > DateTime.Now.Year + 50)
            //{
            //    isValid = false;
            //}

            return new DateParseResult()
            {
                Value = dt,
                IsValid = isValid
            };
        }
    }



    public static class DateFormats
    {
        public static string ddmmyyyy { get; } = "dd.MM.yyyy";
        public static string ddmmyyyySlashes { get; } = "dd/MM/yyyy";
        public static string XLSXParsedDate { get; } = "MM/dd/yyyy hh:mm:ss tt";
        public static string ddmmyyyyHHmmss { get; } = "dd.MM.yyyy HH:mm:ss";
        public static string MMddyyyyHHmmss { get; } = "MM.dd.yyyy HH.mm.sss";
        public static string ddMMyyyyHHmm { get; } = "dd.MM.yyyy HH:mm";

        public static string ddMMyyyyActPrint { get; } = "dd/MM/yyyy";
        public static string ddMMyyyyHHmmFile { get; } = "dd.MM.yyyy HH-mm";

        public static string HHmm { get; } = "HH:mm";
        public static string HH { get; } = "HH";
        public static string HHmmssddMMyyyy { get; } = "HH:mm:ss dd.MM.yyyy";
        public static string JsonActValue { get; } = "yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz";
    }


    public static class DateFormatExtensions
    {
        public static string ToInvariantString(this DateTime date, string format = "dd.MM.yyyy")
        {
            return date.ToString(format, CultureInfo.InvariantCulture);
        }


        public static string ToInvariantString(this DateTime? date, string format = "dd.MM.yyyy")
        {
            if (!date.HasValue)
            {
                return string.Empty;
            }

            return date.Value.ToString(format, CultureInfo.InvariantCulture);
        }
    }

    public static class DateParseResultExtensions
    {
        public static DateParseResult LaterThen(this DateParseResult endDate, DateParseResult startDate, string errMessage = null)
        {
            if (!endDate.IsValid || !startDate.IsValid)
            {
                endDate.IsValid = false;
                return endDate;
            }

            endDate.IsValid = endDate.Value > startDate.Value;
            if (!endDate.IsValid && !string.IsNullOrEmpty(errMessage))
            {
                endDate.ErrorMessage += $", {errMessage}";
            }

            return endDate;
        }

        public static CombinedDateParseResult LaterThen(this CombinedDateParseResult endDate, CombinedDateParseResult startDate, string errMessage = null)
        {
            if (!endDate.IsValid || !startDate.IsValid)
            {
                endDate.IsValid = false;
                return endDate;
            }

            endDate.IsValid = endDate.Value > startDate.Value;

            if (!endDate.IsValid && !string.IsNullOrEmpty(errMessage))
            {
                endDate.DateResult.ErrorMessage += $", {errMessage}";
                endDate.TimeResult.ErrorMessage += $", {errMessage}";
            }

            return endDate;
        }
    }




    public class DoubleParsers
    {
        public DoubleParseResult FromStr(string doubleStr)
        {
            double value;

            doubleStr = doubleStr.Replace(',', '.');

            bool isParsed = double.TryParse(doubleStr, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
            if (!isParsed)
            {
                return new DoubleParseResult() { Value = double.NaN, IsValid = false };
            }
            return new DoubleParseResult() { Value = value, IsValid = true };
        }
    }

    public class DoubleParseResult : IInputParseResult
    {

        public bool IsValid { get; set; }
        public double Value { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class DateParseResult : IInputParseResult
    {

        public bool IsValid { get; set; }
        public DateTime Value { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class CombinedDateParseResult
    {
        public DateParseResult DateResult { get; set; }
        public DateParseResult TimeResult { get; set; }
        public DateTime Value { get; set; }
        public bool IsValid { get; set; }
    }

    // 
    public static class ParseDateForReport
    {
        public static DateTime GetDate(int year)
        {
            var date = DateTime.Now.AddYears(-year);
            date = date.AddMonths(-DateTime.Now.Month);
            date = date.AddDays(-DateTime.Now.Day);
            var dateTo = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);

            return dateTo;
        }

        public static DateTime GetDateForMaxAge(int age)
        {
            var date = DateTime.Now.AddYears(-age + 1).AddSeconds(-1);
            date = date.AddMonths(-DateTime.Now.Month);
            date = date.AddDays(-DateTime.Now.Day);
            var dateTo = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);

            return dateTo;
        }

    }

    public interface IInputParseResult
    {
        bool IsValid { get; }
        string ErrorMessage { get; }
    }

}


