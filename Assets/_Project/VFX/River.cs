using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

namespace DualityGame.VFX
{
    [AddComponentMenu("Duality/River")]
    [RequireComponent(typeof(SplineContainer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteAlways]
    public class River : MonoBehaviour
    {
        [SerializeField] [Range(2, 100)] private int _sections;
        [SerializeField] [Range(0.1f, 5f)] private float _width = 1f;

        private SplineContainer _spline;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;
        private Vector3[] _vertices;
        private static readonly int Length = Shader.PropertyToID("_Length");
        private static readonly int Width = Shader.PropertyToID("_Width");

        private void Awake()
        {
            Setup();
            UpdateMesh();
        }

        private void OnEnable() => Spline.Changed += OnSplitChanged;
        private void OnDisable() => Spline.Changed -= OnSplitChanged;

        private void OnSplitChanged(Spline spline, int knotIndex, SplineModification splineModification)
        {
            if (_spline == null) return;
            if (!_spline.Splines.Contains(spline)) return;
            UpdateMesh();
            
            _meshRenderer.sharedMaterial.SetFloat(Length, _spline.CalculateLength());
        }

        private void UpdateMesh()
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _meshFilter.mesh = _mesh;
            }

            if (_vertices == null || _vertices.Length != (_sections + 1) * 2)
            {
                _vertices = new Vector3[(_sections+1) * 2];
                var uv = new Vector2[_vertices.Length];
                var normals = new Vector3[_vertices.Length];
                var triangles = new int[_sections * 6];
            
                for (var i = 0; i <= _sections; i++)
                {
                    normals[i*2] = Vector3.up;
                    normals[i*2+1] = Vector3.up;
                
                    var t = i / (float)_sections;
                    uv[i*2] = new Vector2(0, t);
                    uv[i*2+1] = new Vector2(1, t);
                
                    if (i == _sections) continue;
                    triangles[i * 6] = i * 2 + 2;
                    triangles[i * 6 + 1] = i * 2 + 1;
                    triangles[i * 6 + 2] = i * 2;
                    triangles[i * 6 + 3] = i * 2 + 3;
                    triangles[i * 6 + 4] = i * 2 + 1;
                    triangles[i * 6 + 5] = i * 2 + 2;
                }
            
                _mesh.Clear();
                _mesh.vertices = _vertices;
                _mesh.uv = uv;
                _mesh.normals = normals;
                _mesh.triangles = triangles;
            }
            RecalculateVertices();
            _mesh.vertices = _vertices;
            _mesh.RecalculateBounds();
        }

        private void RecalculateVertices()
        {
            var offset = transform.position;
            for (var i = 0; i <= _sections; i++)
            {
                var t = i / (float)_sections;

                _spline.Evaluate(t, out var pos, out var tangent, out var up);
                var orthogonal =  Vector3.Cross(tangent, up).normalized * (_width * 0.5f);

                _vertices[i * 2] = (Vector3) pos - offset + orthogonal;
                _vertices[i * 2+1] = (Vector3) pos - offset - orthogonal;
            }
        }

        private void Setup()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _spline = GetComponent<SplineContainer>();
            if (_mesh == null) _mesh = new Mesh();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () => _meshFilter.mesh = _mesh;
            #else
            _meshFilter.mesh = _mesh;
            #endif
            _meshRenderer.sharedMaterial.SetFloat(Width, _width);
            _meshRenderer.sharedMaterial.SetFloat(Length, _spline.CalculateLength());
        }

        private void OnValidate()
        {
            Setup();
            UpdateMesh();
        }
        
        #if UNITY_EDITOR
        private const string DEFAULT_NAME = "River";
        
        [UnityEditor.MenuItem("GameObject/Duality/River", false, 10)]
        private static void CreateSceneGroupColdStartInitializer(UnityEditor.MenuCommand menuCommand)
        {
            var go = new GameObject(DEFAULT_NAME, typeof(River));
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }
        #endif
    }
}
