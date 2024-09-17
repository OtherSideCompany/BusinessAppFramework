using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OtherSideCore.Infrastructure.Context
{
   public class SqlServerDbContext : OtherSideCoreDbContext
   {
      #region Fields



      #endregion

      #region Properties

      public string UserId { get; set; }
      public string DataSource { get; set; }
      public string InitialCatalog { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public SqlServerDbContext()
      {

      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         var connectionString = $"User ID={UserId};";
         connectionString += "Persist Security Info=False;";
         connectionString += $"Data Source = {DataSource};";
         connectionString += "Integrated Security=SSPI;";
         connectionString += $"Initial Catalog={InitialCatalog};";
         connectionString += "TrustServerCertificate = True;";
         connectionString += "App=EntityFramework";

         optionsBuilder.UseSqlServer(connectionString);
      }

      #endregion
   }
}
