using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BusinessAppFramework.Infrastructure.Context
{
   public class SqlServerDbContext : OtherSideCoreDbContext
   {
      #region Fields

      protected string _password;

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
         var builder = new SqlConnectionStringBuilder
         {
            UserID = "app_admin",
            Password = _password,
            DataSource = DataSource,
            InitialCatalog = InitialCatalog,
            TrustServerCertificate = true,
            Encrypt = true,
            PersistSecurityInfo = false,
            ApplicationName = "EntityFramework"
         };

         optionsBuilder.UseSqlServer(builder.ConnectionString);
      }

      #endregion
   }
}
