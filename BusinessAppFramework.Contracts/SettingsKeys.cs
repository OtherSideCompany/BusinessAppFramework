namespace BusinessAppFramework.Contracts
{
    public static class SettingsKeys
    {
        public const string Workspace = "settings-workspace";
        public const string SettingsKey = "settings-configure";
        public static string For(Type settingsType) => KebabStringFormatter.ToKebab(settingsType.Name);
    }
}
