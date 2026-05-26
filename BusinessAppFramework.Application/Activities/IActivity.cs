namespace BusinessAppFramework.Application.Activities
{
    public interface IActivity
    {
        string ActivityKey { get; set; }
        string TargetWorkspaceKey { get; set; }
        string TargetWorkspaceConstraintKey { get; set; }
        int Count { get; set; }
        Task ComputeCountAsync();
    }
}
