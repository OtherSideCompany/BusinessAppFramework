namespace BusinessAppFramework.Application.Settings
{
    /// <summary>
    /// Base class for module settings: a flat set of parameters with their default values.
    /// Derive it in a module, declare public properties with initializers as defaults, and
    /// register the type in IModuleBootstrapper.RegisterSettings.
    /// </summary>
    public abstract class ModuleSettings
    {
    }
}
