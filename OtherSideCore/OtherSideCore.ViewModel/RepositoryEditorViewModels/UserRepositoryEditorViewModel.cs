using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.ViewModel;
using System.Threading.Tasks;
using OtherSideCore.Domain.ModelObjects;
using System.Windows;

namespace OtherSideCore.ViewModel.RepositoryEditorViewModels
{
   public class UserRepositoryEditorViewModel<T> : RepositoryEditorViewModel<T> where T : User, new()
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands

      public AsyncRelayCommand<T> ResetUserPasswordAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public UserRepositoryEditorViewModel(IRepositoryFactory repositoryFactory, User autenticatedUser, IModelObjectViewModelFactory modelObjectViewModelFactory) : base(repositoryFactory, autenticatedUser, modelObjectViewModelFactory)
      {
         ResetUserPasswordAsyncCommand = new AsyncRelayCommand<T>(ResetUserPasswordAsync);
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods

      private async Task ResetUserPasswordAsync(T user)
      {
         var message = "Réinitialiser le mot de passe de l'utilisateur " + user.UserName.Value + " ?";

         if (MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;

         user.ResetPassword();
         var userRepository = _repositoryFactory.CreateUserRepository<T>();
         await userRepository.SaveAsync(user, _authenticatedUser.Id.Value);

         MessageBox.Show("Mot de passe réinitialisé", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
      }

      #endregion
   }
}
