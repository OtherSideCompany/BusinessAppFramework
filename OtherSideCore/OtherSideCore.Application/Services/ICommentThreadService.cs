using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Services
{
   public interface ICommentThreadService
   {
      Task<int?> GetCommentThreadIdAsync(ICommentThreadContainer commentThreadContainer);
      Task DeleteCommentThreadAsync(int commentThreadId);
   }
}
