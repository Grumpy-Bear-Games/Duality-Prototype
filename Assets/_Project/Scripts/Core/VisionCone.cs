using UnityEngine;

namespace DualityGame.Core
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class VisionCone : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private Transform _center;
        
        [Header("Vision cone")]
        [SerializeField] private float _range = 1f;
        [SerializeField, Range(0, 180)] private float _fov = 45;
        [SerializeField, Delayed, Range(1, 60)] private int _precision = 2;
        [SerializeField] private LayerMask _layerMask;

        #region Properties
        public float Range
        {
            get => _range;
            set
            {
                _range = value;
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) UpdateVisionCone();
#endif
            }
        }

        public float FOV
        {
            get => _fov;
            set
            {
                _fov = value;
                RecalculateOriginalVertices();
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) UpdateVisionCone();
#endif
            }
        }
        
        public int Precision
        {
            get => _precision;
            set
            {
                if (value < 1) return;
                _precision = value;
                InitializeMesh();
            }
        }

        public LayerMask LayerMask
        {
            get => _layerMask;
            set
            {
                _layerMask = value;
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) UpdateVisionCone();
#endif
            }
        }

        #endregion

        private Mesh _mesh;
        private Vector3[] _normalizedVertices;

        private void Awake()
        {
            _mesh = new Mesh();
            _meshFilter.mesh = _mesh;
            
            InitializeMesh();
        }

        private void LateUpdate() => UpdateVisionCone();

        private void RecalculateOriginalVertices()
        {
            if (_normalizedVertices == null) return;
            _normalizedVertices[0] = Vector3.zero;
            for (var i = 0; i < _precision + 1; i++)
            {
                var start = -_fov / 2;
                _normalizedVertices[i+1] = Quaternion.AngleAxis(start + i * (_fov / _precision), Vector3.up) * Vector3.forward;
            }
        }

        private void InitializeMesh()
        {
            _normalizedVertices = new Vector3[_precision + 2];
            RecalculateOriginalVertices();
            
            var triangles = new int[_precision * 3];
            var normals = new Vector3[_precision + 2];
            var uv = new Vector2[_precision + 2];
            
            for (var i = 0; i < _precision + 2; i++)
            {
                normals[i] = Vector3.up;
            }

            for (var i = 0; i < _precision; i++)
            {
                triangles[i*3] = 0;
                triangles[i*3 + 1] = i + 1;
                triangles[i*3 + 2] = i + 2;
            }

            _mesh.Clear();
            _mesh.vertices = _normalizedVertices;
            _mesh.uv = uv;
            _mesh.normals = normals;
            _mesh.triangles = triangles;
        }

        private void UpdateVisionCone()
        {
            if (_mesh == null) return;
            var pos = _center.position;

            var vertices = _mesh.vertices;
            var uv = _mesh.uv;
            
            for (var i=1; i<vertices.Length; i++) {
                var ray = _center.rotation * vertices[i];
                if (Physics.Raycast(pos, ray, out var hit, _range, _layerMask))
                {
                    vertices[i] = _normalizedVertices[i] * hit.distance;
                    uv[i] = Vector2.right * (hit.distance / _range);
                }
                else
                {
                    vertices[i] = _normalizedVertices[i] * _range;
                    uv[i] = Vector2.right;
                }
            }

            _mesh.vertices = vertices;
            _mesh.uv = uv;
            _mesh.RecalculateBounds();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
#endif
            _mesh ??= new Mesh();
            _meshFilter.mesh = _mesh;
            InitializeMesh();
            UpdateVisionCone();
        }
        
        private void Reset()
        {
            _center = transform;
            _meshFilter = GetComponent<MeshFilter>();
        }
    }
}
