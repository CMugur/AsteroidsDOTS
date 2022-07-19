using DOTS_Exercise.Utils;
using System;

namespace DOTS_Exercise.Managers
{
    public abstract class ManagerBase : IDisposable
    {
        protected Observer _observer;

        public ManagerBase(Observer observer)
        {
            _observer = observer;
        }

        public abstract void Initialize();
        public virtual void OnUpdate() { }
        public virtual void Dispose() { }
    }
}