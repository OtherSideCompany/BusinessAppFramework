using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Logger
{
   public static class Logger
   {
      public static void ConfigureLogger(ILoggingBuilder builder, string logFile)
      {
         var logger = new LoggerConfiguration().MinimumLevel.Debug()
                                               .WriteTo.File(logFile,
                                                             outputTemplate: "{Timestamp:HH:mm:ss} : [{Level:w3}] {Message:lj}{NewLine}{Exception}")
                                               .CreateLogger();
         builder.AddSerilog(logger);
      }
   }
}
