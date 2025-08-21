using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application
{
   public interface IEntity
   {
      int Id { get; set; }
      HistoryInfo HistoryInfo { get; set; }
   }
}
