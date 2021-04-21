using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFramework.Utilities
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 입력된 요일 한국어 문자열로 변환
        /// </summary>
        /// <param name="weekday"></param>
        /// <returns></returns>
        public static string GetKorWeeDay(DayOfWeek weekday)
        {
            switch (weekday)
            {
                case DayOfWeek.Sunday:
                    return "일";
                case DayOfWeek.Monday:
                    return "월";
                case DayOfWeek.Tuesday:
                    return "화";
                case DayOfWeek.Wednesday:
                    return "수";
                case DayOfWeek.Thursday:
                    return "목";
                case DayOfWeek.Friday:
                    return "금";
                default:
                    return "토";
            }
        }

    }
}
