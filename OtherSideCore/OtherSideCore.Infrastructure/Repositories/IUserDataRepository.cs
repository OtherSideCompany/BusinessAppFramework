using OtherSideCore.Infrastructure.Entities;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Repositories
{
   public interface IUserDataRepository<T> : IDataRepository<T> where T : User
   {
      Task<T> GetUserByCredentials(string userName, string passwordHash);
   }
}
