using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DOTS_Exercise.UI.Screens
{
    public class GameOverScreen : ScreenBase
    {
        [SerializeField] private Button _newGameButton;

        private void Awake()
        {
            _newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        }

        private void OnNewGameButtonClicked()
        {
            SceneManager.LoadScene(0);
        }
    }
}