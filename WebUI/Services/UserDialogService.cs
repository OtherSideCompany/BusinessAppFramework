using Application.Interfaces;
using Application.Services;
using MudBlazor;
using WebUI.Components.Dialog;

namespace WebUI.Services
{
   public class UserDialogService : IUserDialogService
   {
      #region Fields

      private readonly IDialogService _dialogService;
      private readonly ISnackbar _snackbar;
      private readonly ILocalizedStringService _localizedStringService;

      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public UserDialogService(IDialogService dialogService, ISnackbar snackbar, ILocalizedStringService localizedStringService)
      {
         _dialogService = dialogService;
         _snackbar = snackbar;
         _localizedStringService = localizedStringService;
      }

      #endregion

      #region Public Methods

      public async Task<bool> ConfirmAsync(string message)
      {
         var parameters = new DialogParameters
            {
                { nameof(ConfirmDialog.Message), message }
            };

         var options = new DialogOptions()
         {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.ExtraSmall
         };

         var dialog = await _dialogService.ShowAsync<ConfirmDialog>(_localizedStringService.Get(Contracts.StringKeys.ConfirmationKey), parameters, options);
         var result = await dialog.Result;

         return result != null && !result.Canceled;
      }

      public void Error(string message)
      {
         _snackbar.Add(message, Severity.Error);
      }

      public void Show(string message)
      {
         _snackbar.Add(message, Severity.Info);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
