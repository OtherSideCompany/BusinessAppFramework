using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model
{
   public abstract class ModelObjectBase : ObservableObject, IDisposable
   {
      #region Fields

      private int m_Id;
      private bool m_IsLoading;
      private bool m_IsDirty;

      #endregion

      #region Properties

      public Guid guid { get; set; }

      public int Id
      {
         get
         {
            return m_Id;
         }
         set
         {
            if (value != m_Id)
            {
               m_Id = value;
               OnPropertyChanged("Id");
               OnPropertyChanged("IsCreated");
            }
         }
      }

      public bool IsLoading
      {
         get
         {
            return m_IsLoading;
         }
         set
         {
            if (value != m_IsLoading)
            {
               m_IsLoading = value;
               OnPropertyChanged("IsLoading");
            }
         }
      }

      public bool IsCreated
      {
         get
         {
            return Id != 0;
         }
      }

      public bool IsDirty
      {
         get
         {
            return m_IsDirty;
         }
         set
         {
            if (value != m_IsDirty)
            {
               m_IsDirty = value;
               OnPropertyChanged("IsDirty");
            }
         }
      }

      #endregion

      #region Constructor

      public ModelObjectBase()
      {
         guid = Guid.NewGuid();
      }

      #endregion

      #region Virtual Methods

      public virtual bool MatchFilter(List<string> filters, bool extendedSearch)
      {
         return false;
      }

      public abstract void Load();

      public abstract bool CanSaveChanges();

      public abstract bool CanCancelChanges();

      public abstract void Save();

      public abstract bool CanBeDeleted();

      public virtual void Delete()
      {
         IsLoading = true;
         Id = 0;
         IsLoading = false;
      }  

      public abstract void Dispose();

      #endregion

      #region Methods

      public override bool Equals(object obj)
      {
         var item = obj as ModelObjectBase;

         if (item == null)
         {
            return false;
         }

         if (Id == 0 && item.Id == 0)
         {
            return guid == item.guid;
         }
         else
         {
            return Id == item.Id;
         }
      }

      #endregion
   }
}
