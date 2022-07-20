using DOTS_Exercise.Data.Settings;
using DOTS_Exercise.Data.Settings.Attributes;
using DOTS_Exercise.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DOTS_Exercise.Managers
{
    public class CoreManager : MonoBehaviour
    {
        private Observer _observer = new Observer();

        private readonly List<Type> _managerTypes = new List<Type>() 
        { 
            typeof(UnitManager),
            typeof(SystemsManager),
            typeof(GameplayManager),
            typeof(ScreensManager)
        };

        private List<ManagerBase> _managers;

        [SerializeField] private List<SettingsScriptableObject> _settings;

        private void Awake()
        {
            InstantiateManagers(_managerTypes, ref _managers);
            InjectSettings(_settings, _managers);
            InitializeManagers(_managers);
        }

        private void Update()
        {
            _managers.ForEach(m => m.OnUpdate());
        }

        private void InstantiateManagers(List<Type> managerTypes, ref List<ManagerBase> managers)
        {
            DisposeManagers(managers);
            if (managers == null)
            {
                managers = new List<ManagerBase>();
            }

            foreach (var managerType in managerTypes)
            {
                var cInfo = managerType.GetConstructor(new[] { typeof(Observer) });
                if (!(cInfo?.Invoke(new object[] { _observer }) is ManagerBase managerInstance))
                {
                    Debug.LogWarning($"Failed to instantiate manager of type {managerType}");
                    continue;
                }
                managers.Add(managerInstance);
            }
        }

        private void InjectSettings(List<SettingsScriptableObject> settings, List<ManagerBase> managers)
        {
            foreach (var setting in settings)
            {
                var attributes = setting.GetType().GetCustomAttributes()
                    .Where(a => a.GetType() == typeof(SettingsAttribute)).ToList();
                foreach (var attribute in attributes)
                {
                    var interfaceType = (attribute as SettingsAttribute).Interface;
                    var methodName = (attribute as SettingsAttribute).SetSettingsMethodName;

                    foreach (var manager in managers)
                    {
                        if (!interfaceType.IsAssignableFrom(manager.GetType()))
                        {
                            continue;
                        }

                        manager.GetType().GetMethod(methodName).Invoke(manager, new object[1] { setting });
                    }
                }
            }
        }

        private void InitializeManagers(List<ManagerBase> managers)
        {
            managers.ForEach(m => m.Initialize());
        }

        private void DisposeManagers(List<ManagerBase> managers)
        {
            if (managers == null)
            {
                return;
            }

            managers.ForEach(m => m.Dispose());
            managers.Clear();
        }

        private void OnDestroy()
        {
            DisposeManagers(_managers);
            _observer.Dispose();
            _observer = null;
        }
    }
}