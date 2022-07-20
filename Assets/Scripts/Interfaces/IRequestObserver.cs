namespace DOTS_Exercise.Utils.Interfaces
{
    public interface IRequestObserver
    {
        Observer Observer { get; set; }
        void SetObserver(Observer observer);
    }
}