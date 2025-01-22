using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.Factories;
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

      public bool IsInitializingProperties { get; set; }

      public string CreationDescription => GetHistoryDescription(DomainObject.CreationDate, DomainObject.CreatedByName);

      public string ModificationDescription => GetHistoryDescription(DomainObject.LastModifiedDateTime, DomainObject.LastModifiedByName);

      #endregion

      #region Commands

      public RelayCommand ToggleExpandCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectViewModel(DomainObject domainObject,
                                   IGlobalDataService globalDataService,
                                   IMapper mapper,
                                   IDomainObjectViewModelFactory domainObjectViewModelFactory)
      {
         _globalDataService = globalDataService;
         _mapper = mapper;
         _domainObjectViewModelFactory = domainObjectViewModelFactory;

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
         IsInitializingProperties = true;

         _mapper.Map(DomainObject, this);

         IsInitializingProperties = false;
      }

      public virtual void SetPropertiesToDomainObject()
      {
         _mapper.Map(this, DomainObject);
      }

      public virtual void CopyPropertiesToDomainObject(DomainObject domainObject)
      {
         var id = domainObject.Id;
         var creationDate = domainObject.CreationDate;
         var createdById = domainObject.CreatedById;
         var createdByName = domainObject.CreatedByName;
         var lastModifiedDateTime = domainObject.LastModifiedDateTime;
         var lastModifiedById = domainObject.LastModifiedById;
         var lastModifiedByName = domainObject.LastModifiedByName;

         _mapper.Map(this, domainObject);

         domainObject.Id = id;
         domainObject.CreationDate = creationDate; 
         domainObject.CreatedById = createdById;
         domainObject.CreatedByName = createdByName;
         domainObject.LastModifiedDateTime = lastModifiedDateTime;
         domainObject.LastModifiedById = lastModifiedById;
         domainObject.LastModifiedByName = lastModifiedByName;
      }

      public void RefreshTrackingInfos()
      {
         OnPropertyChanged(nameof(CreationDescription));
         OnPropertyChanged(nameof(ModificationDescription));
      }

      #endregion

      #region private Methods

      private string GetHistoryDescription(DateTime dateTime, string? userName)
      {
         var creationDescription = "";

         if (!String.IsNullOrEmpty(userName))
         {
            creationDescription += userName;
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
