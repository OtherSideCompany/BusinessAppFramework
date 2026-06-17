using BusinessAppFramework.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Reflection;

namespace BusinessAppFramework.WebUI.Components.General
{
    public class MultiLevelDataGrid<T> : ComponentBase where T : IComposite<T>
    {
        #region Fields

        protected List<T> _compositeObjects = new();
        protected IEnumerable<T> _visibleCompositeObjects => CollectVisible(_compositeObjects);
        protected readonly HashSet<int> _expandedIds = new();

        #endregion

        #region Properties

        [Parameter] public EventCallback<PropertyInfo> OnChanged { get; set; }

        #endregion

        #region Events



        #endregion

        #region Constructor

        public MultiLevelDataGrid()
        {

        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods

        protected async Task ToggleExpandedAsync(IComposite<T> compositeObject)
        {
            var id = compositeObject.Id;

            if (!_expandedIds.Add(id))
            {
                _expandedIds.Remove(id);
                return;
            }
        }

        protected bool IsExpanded(IComposite<T> compositeObject)
        {
            return _expandedIds.Contains(compositeObject.Id);
        }


        protected static bool RemoveCompositeObject(IList<T> compositeObjects, T target)
        {
            if (compositeObjects.Remove(target))
                return true;

            foreach (var compositeObject in compositeObjects)
            {
                if (RemoveCompositeObject(compositeObject.Children, target))
                    return true;
            }

            return false;
        }

        protected async Task<DataGridEditFormAction> OnCommitedItemChanged(IComposite<T> compositeObject)
        {
            await OnChanged.InvokeAsync();
            return DataGridEditFormAction.Close;
        }

        protected static IEnumerable<T> Flatten(IList<T> compositeObjects)
        {
            foreach (var editor in compositeObjects)
            {
                yield return editor;

                foreach (var descendant in Flatten(editor.Children))
                {
                    yield return descendant;
                }
            }
        }

        private IEnumerable<T> CollectVisible(IList<T> compositeObjects)
        {
            foreach (var compositeObject in compositeObjects)
            {
                yield return compositeObject;

                if (_expandedIds.Contains(compositeObject.Id))
                {
                    foreach (var descendant in CollectVisible(compositeObject.Children))
                        yield return descendant;
                }
            }
        }

        protected void ExpandAll()
        {
            foreach (var editor in Flatten(_compositeObjects).Where(e => e.HasChildren))
                _expandedIds.Add(editor.Id);
        }

        protected void CollapseAll()
        {
            _expandedIds.Clear();
        }

        #endregion
    }
}
