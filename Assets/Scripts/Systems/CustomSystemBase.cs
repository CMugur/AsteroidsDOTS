using DOTS_Exercise.Utils;
using Unity.Entities;

namespace DOTS_Exercise.ECS.Systems
{
    public abstract class CustomSystemBase : SystemBase
    {
        protected Observer _observer;
        public void SetObserver(Observer observer)
        {
            _observer = observer;
        }
    }
}