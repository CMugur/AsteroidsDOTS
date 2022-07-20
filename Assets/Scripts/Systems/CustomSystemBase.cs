using DOTS_Exercise.Utils;
using DOTS_Exercise.Utils.Interfaces;
using Unity.Entities;

namespace DOTS_Exercise.ECS.Systems
{
    public abstract class CustomSystemBase : SystemBase, IRequestObserver
    {
        public Observer Observer { get; set; }

        public void SetObserver(Observer observer)
        {
            Observer = observer;
        }
    }
}