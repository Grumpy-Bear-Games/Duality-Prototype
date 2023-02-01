using DualityGame.Player;

namespace DualityGame.Core
{
    public interface IKillable
    {
        void Kill(CauseOfDeath causeOfDeath);
    }
}
