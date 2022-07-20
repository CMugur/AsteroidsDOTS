using DOTS_Exercise.Utils;

namespace DOTS_Exercise.Managers
{
    public class GameplayManager : ManagerBase
    {
        private int _score = 0;

        public GameplayManager(Observer observer) : base(observer) { }

        public override void Initialize()
        {
            _observer.AddListener(Events.Trigger_OnUnitDied, OnUnitDied);
        }

        private void OnUnitDied(object arg)
        {
            UnitDiedDTO dto = arg as UnitDiedDTO;
            _score += dto.UnitComponent.ScoreAddedOnDeath;
            _observer.Trigger(Events.Trigger_OnScoreChanged, _score);

            if (dto.UnitComponent.UnitType == UnitTypes.Player && dto.UnitComponent.Lives == 1)
            {
                OnGameEnded();
            }
        }

        private void OnGameEnded()
        {
            _observer.RemoveListener(Events.Trigger_OnUnitDied, OnUnitDied);
            _observer.Trigger(Events.Trigger_OnGameEnded, null);
        }
    }
}