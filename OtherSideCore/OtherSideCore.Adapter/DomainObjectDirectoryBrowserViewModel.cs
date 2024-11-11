
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using System.Collections.ObjectModel;
using System.IO;
using static System.Net.WebRequestMethods;

namespace OtherSideCore.Adapter
{
   public class DomainObjectDirectoryBrowserViewModel : ObservableObject, IDisposable, IFileDropZone
   {
      #region Fields

      private DomainObjectViewModel _domainObjectViewModel;
      private IDomainObjectFileService _domainObjectFileService;
      private IUserDialogService _userDialogService;

      private ObservableCollection<FileInfo> _fileInfos;
      private ObservableCollection<DirectoryInfo> _nestedDirectoriesInfos;
      private string _folderName;
      private FileSystemWatcher _folderWatcher;

      #endregion

      #region Properties

      public ObservableCollection<FileInfo> FileInfos
      {
         get => _fileInfos;
         set => SetProperty(ref _fileInfos, value);
      }

      public ObservableCollection<DirectoryInfo> NestedDirectoriesInfos
      {
         get => _nestedDirectoriesInfos;
         set => SetProperty(ref _nestedDirectoriesInfos, value);
      }

      public string FolderName
      {
         get => _folderName;
         set => SetProperty(ref _folderName, value);
      }

      public bool FolderExists => Path.Exists(FolderName);

      public IDomainObjectFileService DomainObjectFileService => _domainObjectFileService;

      #endregion

      #region Commands

      public RelayCommand CreateFolderCommand { get; private set; }
      public RelayCommand OpenFolderCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectDirectoryBrowserViewModel(IDomainObjectFileService domainObjectFileService,
                                                   DomainObjectViewModel domainObjectViewModel,
                                                   IUserDialogService userDialogService)
      {
         _domainObjectViewModel = domainObjectViewModel;
         _domainObjectFileService = domainObjectFileService;
         _userDialogService = userDialogService;

         CreateFolderCommand = new RelayCommand(CreateFolder, CanCreateFolder);
         OpenFolderCommand = new RelayCommand(OpenFolder, CanOpenFolder);

         FolderName = domainObjectFileService.GetAssociatedDirectoryInfo(domainObjectViewModel.DomainObject).FullName;
         InitializeFilesAndDirectories();
         WatchCurrentFolder();

         OnPropertyChanged(nameof(FolderExists));

         NotifyCommandCanExecuteChanged();
      }

      #endregion

      #region Public Methods

      public void DropFiles(List<ManagedFile> managedFiles)
      {
         try
         {
            _domainObjectFileService.CopyFilesInAssociatedFolder(_domainObjectViewModel.DomainObject, managedFiles);
         }
         catch (IOException e)
         {
            _userDialogService.Error("Une erreur s'est produite lors de la copie des fichiers dans le dossier associé.");
         }
      }

      public void Dispose()
      {
         UnwatchCurrentFolder();
      }

      #endregion

      #region Private Methods

      private void InitializeFilesAndDirectories()
      {
         FileInfos = new ObservableCollection<FileInfo>(_domainObjectFileService.GetAssociatedFileInfos(_domainObjectViewModel.DomainObject));
         NestedDirectoriesInfos = new ObservableCollection<DirectoryInfo>(_domainObjectFileService.GetAssociatedNestedDirectoriesInfos(_domainObjectViewModel.DomainObject));
      }

      private void WatchCurrentFolder()
      {
         if (_folderWatcher == null && Directory.Exists(FolderName))
         {
            _folderWatcher = new FileSystemWatcher(FolderName);

            _folderWatcher.NotifyFilter = NotifyFilters.Attributes
                                    | NotifyFilters.CreationTime
                                    | NotifyFilters.DirectoryName
                                    | NotifyFilters.FileName
                                    | NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.Security
                                    | NotifyFilters.Size;

            _folderWatcher.Changed += OnFolderChanged;
            _folderWatcher.Created += OnFolderChanged;
            _folderWatcher.Deleted += OnFolderChanged;
            _folderWatcher.Renamed += OnFolderChanged;

            _folderWatcher.EnableRaisingEvents = true;
         }
      }
      public void UnwatchCurrentFolder()
      {
         if (_folderWatcher != null)
         {
            _folderWatcher.Changed -= OnFolderChanged;
            _folderWatcher.Created -= OnFolderChanged;
            _folderWatcher.Deleted -= OnFolderChanged;
            _folderWatcher.Renamed -= OnFolderChanged;

            _folderWatcher.Dispose();
            _folderWatcher = null;
         }
      }

      private void OnFolderChanged(object sender, FileSystemEventArgs e)
      {
         UnwatchCurrentFolder();
         InitializeFilesAndDirectories();
         WatchCurrentFolder();
      }

      private bool CanOpenFolder()
      {
         return FolderExists;
      }

      private void OpenFolder()
      {
         _domainObjectFileService.OpenFolder(_domainObjectViewModel.DomainObject);
      }

      private bool CanCreateFolder()
      {
         return !FolderExists;
      }

      private void CreateFolder()
      {
         _domainObjectFileService.CreateFolder(_domainObjectViewModel.DomainObject);

         OnPropertyChanged(nameof(FolderExists));
         NotifyCommandCanExecuteChanged();

         FolderName = _domainObjectFileService.GetAssociatedDirectoryInfo(_domainObjectViewModel.DomainObject).FullName;
         WatchCurrentFolder();
      }

      protected void NotifyCommandCanExecuteChanged()
      {
         CreateFolderCommand.NotifyCanExecuteChanged();
         OpenFolderCommand.NotifyCanExecuteChanged();
      }      

      #endregion
   }
}
