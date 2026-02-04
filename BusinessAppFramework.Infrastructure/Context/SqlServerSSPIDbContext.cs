using Microsoft.EntityFrameworkCore;

namespace BusinessAppFramework.Infrastructure.Context
{
   public class SqlServerSSPIDbContext : OtherSideCoreDbContext
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

      public SqlServerSSPIDbContext()
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
