namespace DualityGame.SaveSystem
{
    public interface ISaveableComponent
    {
        object CaptureState();
        void RestoreState(object state);
    }
}
