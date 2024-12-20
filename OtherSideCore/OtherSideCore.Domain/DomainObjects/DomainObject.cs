using System;

namespace OtherSideCore.Domain.DomainObjects
{
   public abstract class DomainObject : IDisposable
   {
      #region Fields



      #endregion

      #region Properties

      public int Id { get; set; }
      public DateTime CreationDate { get; set; }
      public int? CreatedById { get; set; }
      public string? CreatedByName { get; set; }
      public DateTime LastModifiedDateTime { get; set; }
      public int? LastModifiedById { get; set; }
      public string? LastModifiedByName { get; set; }

      #endregion

      #region Constructor

      public DomainObject()
      {
         
      }

      #endregion

      #region Public Methods

      public virtual void LoadDefaultProperties() { }

      public override bool Equals(object obj)
      {
         var item = obj as DomainObject;

         if (item == null)
         {
            return false;
         }

         if (Id == 0 && item.Id == 0)
         {
            return GetHashCode() == item.GetHashCode();
         }
         else
         {
            return Id == item.Id;
         }
      }

      public virtual void Dispose()
      {
         
      }

      #endregion

      #region Private Methods      
     

      #endregion
   }
}
