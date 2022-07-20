using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace DOTS_Exercise.Utils
{
    public class Observer : IDisposable
    {
        #region Events
        private readonly Dictionary<string, UnityEvent<object>> _events = new Dictionary<string, UnityEvent<object>>();
        public void Trigger(string key, object arg)
        {
            CheckExistance(key);
            _events[key].Invoke(arg);
        }

        public void AddListener(string key, UnityAction<object> ac)
        {
            CheckExistance(key);
            _events[key].AddListener(ac);
        }

        public void RemoveListener(string key, UnityAction<object> ac)
        {
            CheckExistance(key);
            _events[key].RemoveListener(ac);
        }

        public void RemoveAllListeners()
        {
            foreach (var _event in _events)
                _event.Value.RemoveAllListeners();
        }
        private void CheckExistance(string key)
        {
            if (!_events.ContainsKey(key))
                _events.Add(key, new UnityEvent<object>());
        }
        #endregion

        #region Request Action Event
        private readonly Dictionary<string, Action<object>> _requestAction = new Dictionary<string, Action<object>>();

        public void AddRequest(string key, Action<object> action)
        {
            if (_requestAction.ContainsKey(key))
            {
                _requestAction.Remove(key);
            }

            _requestAction.Add(key, action);
        }

        public void AddRequest(string key, Action action)
        {
            if (_requestAction.ContainsKey(key))
            {
                _requestAction.Remove(key);
            }
            _requestAction.Add(key, (x) => { action(); });
        }


        public bool TriggerRequest(string key, object arg)
        {
            if (!_requestAction.ContainsKey(key))
            {
                return false;
            }

            _requestAction[key].Invoke(arg);
            return true;
        }
        #endregion

        public void Dispose()
        {
            _requestAction.Clear();
        }
    }
}