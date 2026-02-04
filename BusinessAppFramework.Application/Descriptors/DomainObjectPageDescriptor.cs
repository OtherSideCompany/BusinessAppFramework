using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Descriptors
{
   public class DomainObjectPageDescriptor<T> : WorkspaceDescriptor where T : DomainObject
   {
      #region Fields

      public Type ContentComponentType { get; init; } = default!;

      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public DomainObjectPageDescriptor()
      {

      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
