using BusinessAppFramework.Application.Descriptors;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IDomainObjectCreationDialogService
    {
        Task<int?> CreateAsync(IDomainObjectSelectorDescriptor descriptor);
    }
}
