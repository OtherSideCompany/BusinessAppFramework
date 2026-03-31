namespace BusinessAppFramework.Application.Interfaces
{
   public interface ICalendarService
   {
      int GetCurrentWeekNumber();
      int GetWeekNumber(DateTime datetime);
   }
}
