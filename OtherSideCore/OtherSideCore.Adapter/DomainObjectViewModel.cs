using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Adapter
{
   public class DomainObjectViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private bool _isSelected;
      private bool _isExpanded;
      private DomainObject _domainObject;

      protected IGlobalDataService _globalDataService;
      protected IMapper _mapper;
      protected IDomainObjectViewModelFactory _domainObjectViewModelFactory;
      protected IWindowService _windowService;

      protected List<DomainObjectViewModel> _nestedDomainObjectViewModels;

      #endregion

      #region Properties

      public IGlobalDataService GlobalDataService
      {
         get => _globalDataService;
         set => SetProperty(ref _globalDataService, value);
      }

      public bool IsSelected
      {
         get => _isSelected;
         set => SetProperty(ref _isSelected, value);
      }

      public bool IsExpanded
      {
         get => _isExpanded;
         set => SetProperty(ref _isExpanded, value);
      }

      public DomainObject DomainObject
      {
         get => _domainObject;
         set => SetProperty(ref _domainObject, value);
      }

      public HashSet<string> MonitoredProperties { get; set; }

      public string CreationDescription => GetHistoryDescription(DomainObject.CreationDate, DomainObject.CreatedBy);

      public string ModificationDescription => GetHistoryDescription(DomainObject.LastModifiedDateTime, DomainObject.LastModifiedBy);

      #endregion

      #region Commands

      public RelayCommand DisplayInExternalWindowCommand { get; private set; }

      public RelayCommand ToggleExpandCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectViewModel(DomainObject domainObject,
                                   IGlobalDataService globalDataService,
                                   IMapper mapper,
                                   IDomainObjectViewModelFactory domainObjectViewModelFactory,
                                   IWindowService windowService)
      {
         _globalDataService = globalDataService;
         _mapper = mapper;
         _domainObjectViewModelFactory = domainObjectViewModelFactory;
         _windowService = windowService;

         DisplayInExternalWindowCommand = new RelayCommand(DisplayInExternalWindow);
         ToggleExpandCommand = new RelayCommand(() => IsExpanded = !IsExpanded);

         DomainObject = domainObject;

         MonitoredProperties = new HashSet<string>();

         _nestedDomainObjectViewModels = new List<DomainObjectViewModel>();

         RefreshTrackingInfos();
      }

      #endregion

      #region Public Methods

      public virtual void ResetState()
      {

      }

      public virtual void Dispose() 
      {
         _nestedDomainObjectViewModels.ToList().ForEach(vm => vm.Dispose());
         _nestedDomainObjectViewModels.Clear();
      }

      public virtual void InitializeProperties()
      {
         _mapper.Map(DomainObject, this);
      }

      public virtual void SetPropertiesToDomainObject()
      {
         _mapper.Map(this, DomainObject);
      }

      public void RefreshTrackingInfos()
      {
         OnPropertyChanged(nameof(CreationDescription));
         OnPropertyChanged(nameof(ModificationDescription));
      }

      #endregion

      #region private Methods

      protected virtual void DisplayInExternalWindow() 
      {
         ((IWindowService)_windowService).ShowDomainObjectViewModelInSubWindow(this);
      }

      private string GetHistoryDescription(DateTime dateTime, User user)
      {
         var creationDescription = "";

         if (user != null)
         {
            creationDescription += user.FirstName + " " + user.LastName;
         }
         else
         {
            creationDescription += "Inconnu";
         }

         creationDescription += dateTime.ToString(", le dd/MM/yyyy à HH:mm");

         return creationDescription;
      }

      #endregion
   }
}
