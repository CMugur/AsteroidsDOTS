using System;
using System.Collections.Generic;

namespace DOTS_Exercise.Utils
{
    public class Observer : IDisposable
    {

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