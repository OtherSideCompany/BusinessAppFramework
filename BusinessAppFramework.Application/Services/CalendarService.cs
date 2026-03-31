using BusinessAppFramework.Application.Interfaces;
using System.Globalization;

namespace BusinessAppFramework.Application.Services
{
   public class CalendarService : ICalendarService
   {
      #region Fields

      private CalendarWeekRule _calendarWeekRule = CalendarWeekRule.FirstFourDayWeek;
      private DayOfWeek _firstDayOfWeek = DayOfWeek.Monday;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public CalendarService()
      {

      }

      #endregion

      #region Public Methods

      public int GetCurrentWeekNumber()
      {
         return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, _calendarWeekRule, _firstDayOfWeek);
      }

      public int GetWeekNumber(DateTime datetime)
      {
         return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(datetime, _calendarWeekRule, _firstDayOfWeek);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
