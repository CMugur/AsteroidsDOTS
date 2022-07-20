namespace DOTS_Exercise.Utils
{
    public class Events
    {
        /// <summary>
        /// Spawning a projectile takes a: <see cref="SpawnProjectileDTO"/>
        /// </summary>
        public const string Request_SpawnPlayerProjectile = "UnitsManager.SpawnPlayerProjectile";
        
        /// <summary>
        /// Spawning a projectile takes a: <see cref="SpawnProjectileDTO"/>
        /// </summary>
        public const string Request_SpawnUFOProjectile = "UnitsManager.SpawnUFOProjectile";

        /// <summary>
        /// Pushing a screen takes a: <see cref="string"/>
        /// </summary>
        public const string Request_PushScreen = "ScreensManager.PushScreen";

        /// <summary>
        /// Triggered when a unit entity died: <see cref="UnitComponent"/>
        /// </summary>
        public const string Trigger_OnUnitDied = "UnitsManager.OnUnitDied";
        
        /// <summary>
        /// Triggered when score changes: <see cref="int"/>
        /// </summary>
        public const string Trigger_OnScoreChanged = "GameplayManager.OnScoreChanged";
        
        /// <summary>
        /// Triggered when the game starts
        /// </summary>
        public const string Trigger_OnGameStart = "GameplayManager.OnGameStart";
        
        /// <summary>
        /// Triggered when the game ends
        /// </summary>
        public const string Trigger_OnGameEnded = "GameplayManager.OnGameEnded";
    }
}