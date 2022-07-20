using DOTS_Exercise.Data.Settings.Attributes;
using DOTS_Exercise.Data.Settings.Interfaces;
using DOTS_Exercise.UI.Screens;
using System.Collections.Generic;
using UnityEngine;

namespace DOTS_Exercise.Data.Settings
{
    [Settings(Interface = typeof(IRequestUISettings), SetSettingsMethodName = "SetUISettings"),
        CreateAssetMenu(fileName = "UI Settings Scriptable Object", menuName = "Scriptable Objects/Settings/UI Settings")]
    public class UISettingsScriptableObject : SettingsScriptableObject
    {
        public ScreenBase ScreenToDisplayOnStart;
        public List<ScreenBase> Screens;
    }
}