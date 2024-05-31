using OtherSideCore.Data.DatabaseFields;
using OtherSideCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Data
{
   public class User : EntityBase
   {
      public int UserId { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string UserName { get; set; }
   }
}
