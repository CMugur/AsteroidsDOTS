using DOTS_Exercise.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DOTS_Exercise.UI.Screens
{
    public class GameplayScreen : ScreenBase
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private List<GameObject> _lifes;

        private void Awake()
        {
            SetScore(0);
            SetLifes(3);
        }

        public override void SetObserver(Observer observer)
        {
            base.SetObserver(observer);
            Observer.AddListener(Events.Trigger_OnScoreChanged, OnScoreChanged);
            Observer.AddListener(Events.Trigger_OnUnitDied, OnUnitDied);
        }

        private void OnScoreChanged(object arg)
        {
            SetScore((int)arg);
        }

        private void OnUnitDied(object arg)
        {
            UnitDiedWithPositionDTO dto = (UnitDiedWithPositionDTO)arg;
            if (dto.UnitComponent.UnitType == UnitTypes.Player)
            {
                SetLifes(dto.UnitComponent.Lives - 1);
            }
        }

        private void SetScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void SetLifes(int lifes)
        {
            for (int i = 0; i < _lifes.Count; i++)
            {
                _lifes[i].gameObject.SetActive(i < lifes);
            }
        }
    }
}