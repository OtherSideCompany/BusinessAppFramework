
using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class Selection : ObservableObject
   {
      #region Fields

      private ISelectable _selectedItem;
      private List<ISelectable> _selectedItems;

      #endregion

      #region Properties

      public SelectionType SelectionType { get; private set; }

      public ISelectable SelectedItem
      {
         get
         {
            if (SelectionType == SelectionType.None)
            {
               return null;
            }
            else if (SelectionType == SelectionType.Single)
            {
               return _selectedItem;
            }
            else if (SelectionType == SelectionType.Multiple)
            {
               return _selectedItems.FirstOrDefault();
            }
            else
            {
               throw new ArgumentException("Unknown selection type " + SelectionType.ToString());
            }
         }
      }

      public List<ISelectable> SelectedItems
      {
         get
         {
            if (SelectionType == SelectionType.None)
            {
               return null;
            }
            else if (SelectionType == SelectionType.Single)
            {
               return null;
            }
            else if (SelectionType == SelectionType.Multiple)
            {
               return _selectedItems;
            }
            else
            {
               throw new ArgumentException("Unknown selection type " + SelectionType.ToString());
            }
         }
      }

      public bool IsSelectionEmpty
      {
         get
         {
            if (SelectionType == SelectionType.None)
            {
               return true;
            }
            else if (SelectionType == SelectionType.Single)
            {
               return _selectedItem == null;
            }
            else if (SelectionType == SelectionType.Multiple)
            {
               return !_selectedItems.Any();
            }
            else
            {
               return true;
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Selection(SelectionType selectionType)
      {
         SelectionType = selectionType;
         _selectedItems = new List<ISelectable>();
      }

      #endregion

      #region Public Methods

      public void SetSelectionType(SelectionType selectionType)
      {
         SelectionType = selectionType;
      }

      public void Select(ISelectable? selectable)
      {
         if (selectable != null)
         {
            if (SelectionType == SelectionType.Single && SelectedItem != selectable)
            {
               Unselect(SelectedItem);
               _selectedItem = selectable;
               _selectedItem.IsSelected = true;
            }
            else if (SelectionType == SelectionType.Multiple && !_selectedItems.Contains(selectable))
            {
               if (_selectedItem == null)
               {
                  _selectedItem = selectable;
               }

               _selectedItems.Add(selectable);
               selectable.IsSelected = true;
            }

            NotifyPropertyChanged();
         }
      }

      public void Unselect(ISelectable selectable)
      {
         if (selectable != null)
         {
            if (SelectionType == SelectionType.Single && selectable == _selectedItem)
            {
               selectable.IsSelected = false;
               _selectedItem = null;
            }
            else if (SelectionType == SelectionType.Multiple && _selectedItems.Contains(selectable))
            {
               selectable.IsSelected = false;
               _selectedItems.Remove(selectable);

               if (selectable.Equals(_selectedItem))
               {
                  _selectedItem = _selectedItems.FirstOrDefault();
               }
            }

            NotifyPropertyChanged();
         }
      }

      public void ClearSelection()
      {
         if (_selectedItem != null)
         {
            _selectedItem.IsSelected = false;
         }

         if (_selectedItems != null)
         {
            _selectedItems.ForEach(vm => vm.IsSelected = false);
         }

         _selectedItem = null;
         _selectedItems?.Clear();

         NotifyPropertyChanged();
      }

      #endregion

      #region Private Methods

      private void NotifyPropertyChanged()
      {
         OnPropertyChanged(nameof(SelectedItem));
         OnPropertyChanged(nameof(SelectedItems));
         OnPropertyChanged(nameof(IsSelectionEmpty));
      }

      #endregion
   }
}
