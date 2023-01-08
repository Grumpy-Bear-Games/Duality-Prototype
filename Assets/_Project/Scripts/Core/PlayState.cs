using Games.GrumpyBear.Core.Observables;

namespace DualityGame.Core
{
    public static class PlayState
    {
        public static readonly Observable<State> Current = new();

        public enum State {
            Moving,
            Talking,
            Inventory,
        }
    }
}
