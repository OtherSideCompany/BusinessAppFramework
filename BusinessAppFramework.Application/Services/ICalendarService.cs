namespace BusinessAppFramework.Application.Services
{
   public interface ICalendarService
   {
      int GetCurrentWeekNumber();
      int GetWeekNumber(DateTime datetime);
   }
}
