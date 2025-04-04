using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Services
{
   public interface ICalendarService
   {
      int GetCurrentWeekNumber();
      int GetWeekNumber(DateTime datetime);
   }
}
