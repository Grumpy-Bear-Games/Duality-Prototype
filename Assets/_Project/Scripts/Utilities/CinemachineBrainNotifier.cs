using Cinemachine;
using DualityGame.Core;
using UnityEngine;

namespace DualityGame.Utilities
{
    [RequireComponent(typeof(CinemachineBrain))]
    public class CinemachineBrainNotifier : NotifierBase<CinemachineBrain, CinemachineBrainObservable> {}
}
