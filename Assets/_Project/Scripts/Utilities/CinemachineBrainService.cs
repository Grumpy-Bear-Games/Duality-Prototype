using Cinemachine;
using UnityEngine;

namespace DualityGame.Utilities
{
    [RequireComponent(typeof(CinemachineBrain))]
    public class CinemachineBrainService : ServiceProxy<CinemachineBrain> { }
}