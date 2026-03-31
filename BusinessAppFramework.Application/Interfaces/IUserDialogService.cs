namespace BusinessAppFramework.Application.Interfaces
{
    public interface IUserDialogService
    {
        Task<bool> ConfirmAsync(string message);
        void SnackError(string message);
        void SnackShow(string message);
        Task DialogErrorAsync(string message);
    }
}
