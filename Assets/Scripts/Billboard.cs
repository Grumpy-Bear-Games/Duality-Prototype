using UnityEngine;

namespace DualityGame
{
    public class Billboard : MonoBehaviour
    {
        private Camera _camera;

        private void Awake() => _camera = Camera.main;

        void Update() => transform.LookAt(_camera.transform.position, Vector3.up);
    }
}
