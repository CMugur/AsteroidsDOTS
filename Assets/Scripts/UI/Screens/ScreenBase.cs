using DOTS_Exercise.Utils;
using DOTS_Exercise.Utils.Interfaces;
using UnityEngine;

namespace DOTS_Exercise.UI.Screens
{
    public class ScreenBase : MonoBehaviour, IRequestObserver
    {
        public Observer Observer { get; set; }

        public virtual void SetObserver(Observer observer)
        {
            Observer = observer;
        }
    }
}