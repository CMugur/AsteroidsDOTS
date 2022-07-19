namespace DOTS_Exercise.Data.Settings.Interfaces
{
    public interface IRequestRenderingSettings
    {
        RenderingSettingsScriptableObject RenderingSettings { get; set; }
        void SetRenderingSettings(RenderingSettingsScriptableObject settings);
    }
}