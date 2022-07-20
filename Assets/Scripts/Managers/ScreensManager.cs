using DOTS_Exercise.Data.Settings;
using DOTS_Exercise.Data.Settings.Interfaces;
using DOTS_Exercise.UI.Screens;
using DOTS_Exercise.Utils;
using System;
using System.Linq;
using UnityEngine;

namespace DOTS_Exercise.Managers
{
    public class ScreensManager : ManagerBase, IRequestUISettings
    {
        public UISettingsScriptableObject UISettings { get; set; }
        public void SetUISettings(UISettingsScriptableObject settings)
        {
            UISettings = settings;
        }

        private ScreenBase _existingScreen;

        public ScreensManager(Observer observer) : base(observer) { }

        public override void Initialize()
        {
            _observer.AddRequest(Events.Request_PushScreen, PushScreen);
            _observer.AddListener(Events.Trigger_OnGameStart, OnGameStart);
            _observer.AddListener(Events.Trigger_OnGameEnded, OnGameEnded);
            PushScreen(UISettings.ScreenToDisplayOnStart);
        }

        private void OnGameStart(object arg)
        {
            PushScreen(typeof(GameplayScreen).ToString());
        }

        private void OnGameEnded(object arg)
        {
            PushScreen(typeof(GameOverScreen).ToString());
        }

        private void PushScreen(object arg)
        {
            PushScreen(arg.ToString());
        }

        private void PushScreen(string type)
        {
            PushScreen(UISettings.Screens.FirstOrDefault(s => s.GetType().ToString() == type));
        }

        private void PushScreen(ScreenBase screen)
        {
            if (screen == null)
            {
                return;
            }

            if (_existingScreen != null)
            {
                GameObject.Destroy(_existingScreen.gameObject);
            }

            _existingScreen = GameObject.Instantiate(screen);
            _existingScreen.SetObserver(_observer);
        }
    }
}