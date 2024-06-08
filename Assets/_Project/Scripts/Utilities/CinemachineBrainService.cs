using Cinemachine;
using DualityGame.ServiceLocator;
using UnityEngine;

namespace DualityGame.Utilities
{
    [RequireComponent(typeof(CinemachineBrain))]
    public class CinemachineBrainService : ServiceProxy<CinemachineBrain> { }
}