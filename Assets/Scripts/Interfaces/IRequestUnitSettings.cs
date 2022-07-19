namespace DOTS_Exercise.Data.Settings.Interfaces
{
    public interface IRequestUnitSettings
    {
        UnitSettingsScriptableObject UnitSettings { get; set; }
        void SetUnitSettings(UnitSettingsScriptableObject settings);
    }
}