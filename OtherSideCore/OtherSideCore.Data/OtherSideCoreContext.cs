using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data
{
   public abstract class OtherSideCoreContext : DbContext
   {
      #region Fields

      

      #endregion

      #region Properties

      

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public OtherSideCoreContext(string connectionString) : base(connectionString + "App=EntityFramework")
      {
         Database.SetInitializer(new MigrateDatabaseToLatestVersion<OtherSideCoreContext, Migrations.Configuration>());
      }

      #endregion

      #region Methods



      #endregion
   }
}
