namespace DOTS_Exercise.Data.Settings.Interfaces
{
    public interface IRequestUISettings
    {
        UISettingsScriptableObject UISettings { get; set; }
        void SetUISettings(UISettingsScriptableObject settings);
    }
}
