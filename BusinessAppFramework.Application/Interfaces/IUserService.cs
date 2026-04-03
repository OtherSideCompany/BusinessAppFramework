namespace BusinessAppFramework.Application.Interfaces
{
    public interface IUserService
    {
        Task<string?> GetUserNameAsync(int userId);
        Task SetProfilePictureAsync(int domainObjectId, byte[] logoBytes);
        Task<byte[]> GetProfilePictureAsync(int domainObjectId);
        Task ChangePasswordAsync(int userId, string newPassword);
    }
}
