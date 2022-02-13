using System;
using System.Collections.Generic;
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
                _meshFilter.mesh.Clear();
            }
        }
        
        public float FOV
        {
            get => _fov;
            set
            {
                _fov = value;
                _meshFilter.mesh.Clear();
            }
        }
        
        public int Precision
        {
            get => _precision;
            set
            {
                _precision = value;
                _meshFilter.mesh.Clear();
            }
        }

        public LayerMask LayerMask
        {
            get => _layerMask;
            set
            {
                _layerMask = value;
                UpdateVisionCone(_meshFilter.mesh);
            }
        }
        #endregion

        
        private readonly List<Vector3> _originalVertices = new();
    
        private void Awake()
        {
            if (_meshFilter.mesh == null) _meshFilter.mesh = new Mesh();
            InitMesh(_meshFilter.mesh);
        }

        private void LateUpdate()
        {
            if (_meshFilter.mesh.vertexCount == 0) InitMesh(_meshFilter.mesh);
            UpdateVisionCone(_meshFilter.mesh);
        }

        private void InitMesh(Mesh mesh)
        {
            if (_precision < 1) return;
        
            _originalVertices.Clear();
        
            var triangles = new List<int>();
            var normals = new List<Vector3>();
            var uv = new List<Vector2>();
        
            _originalVertices.Add(Vector3.zero);
            normals.Add(Vector3.up);
            uv.Add(Vector2.zero);

            var dir = Vector3.forward * _range;
        
            for (var i = 0; i < _precision + 1; i++)
            {
                var start = -_fov / 2;

                _originalVertices.Add(Quaternion.AngleAxis(start + i * (_fov / _precision), Vector3.up) * dir);
                normals.Add(Vector3.up);
                uv.Add(Vector2.right);
            }

            for (var i = 0; i < _precision; i++)
            {
                triangles.Add(0);
                triangles.Add(i + 1);
                triangles.Add(i + 2);
            }

            mesh.Clear();
            mesh.vertices = _originalVertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uv.ToArray();
            mesh.triangles = triangles.ToArray();
        }

        private void UpdateVisionCone(Mesh mesh)
        {
            var pos = _center.position;
            var vertices = new List<Vector3> { Vector3.zero };
            var uv = new List<Vector2> { Vector2.zero };
            for (var i=0; i<_originalVertices.Count; i++) {
                var vertex = _originalVertices[i];
                if (i == 0)
                {
                    vertices.Add(vertex);
                    uv.Add(Vector2.zero);
                    continue;
                }

                var ray = _center.rotation * vertex;
                if (Physics.Raycast(pos, ray, out var hit, _range, _layerMask))
                {
                    vertices.Add(vertex.normalized * hit.distance);
                    uv.Add(Vector2.right * (hit.distance / _range));
                    if (hit.transform.CompareTag("Player"))
                    {
                        Debug.Log("Player spotted!");
                    }
                }
                else
                {
                    vertices.Add(vertex);
                    uv.Add(Vector2.right);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.uv = uv.ToArray();
            mesh.RecalculateBounds();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
#endif
            if (_meshFilter.sharedMesh == null) _meshFilter.sharedMesh = new Mesh();

            InitMesh(_meshFilter.sharedMesh);
            UpdateVisionCone(_meshFilter.sharedMesh);
        }
        
        private void Reset()
        {
            _center = transform;
            _meshFilter = GetComponent<MeshFilter>();
        }
    }
}
