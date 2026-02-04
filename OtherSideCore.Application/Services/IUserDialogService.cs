using OtherSideCore.Application.Services;

namespace OtherSideCore.Appplication.Services
{
    public interface IUserDialogService
    {
        Task<bool> ConfirmAsync(string message);
        void Error(string message);
        void Show(string message);
    }
}
