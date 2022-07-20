using DOTS_Exercise.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace DOTS_Exercise.UI.Screens
{
    public class MainScreen : ScreenBase
    {
        [SerializeField] private Button _playButton;

        private void Awake()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            Observer?.Trigger(Events.Trigger_OnGameStart, null);
        }
    }
}