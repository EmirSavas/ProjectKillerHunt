using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISight : MonoBehaviour
{
    public Transform target;
    public Transform lastSeenTransform;

    [Space(20)] [Header("SETTINGS")]
    // [SerializeField] private Transform refPoint;
    // [SerializeField] private float fov;
    // [SerializeField] private float maxRange;
    // [SerializeField] private float minRange;

    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public Color meshColor = Color.red;
    public int scanFrequency = 30;
    public LayerMask layers;
    public LayerMask occlusionLayers;

    public List<GameObject> Objects
    {
        get
        {
            _objects.RemoveAll(obj => !obj);
            return _objects;
        }
    }
    private readonly List<GameObject> _objects = new List<GameObject>();

    private readonly Collider[] _colliders = new Collider[50];
    private Mesh _mesh;
    
    private int _count;
    private float _scanInterval;
    private float _scanTimer;

    private void Start()
    {
        _scanInterval = 1.0f / scanFrequency;
    }

    private void Update()
    {
        _scanTimer -= Time.deltaTime;
        if (_scanTimer < 0)
        {
            _scanTimer += _scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        _count = Physics.OverlapSphereNonAlloc(transform.position, distance, _colliders, layers, QueryTriggerInteraction.Collide);
        
        _objects.Clear();

        for (int i = 0; i < _count; i++)
        {
            GameObject obj = _colliders[i].gameObject;

            if (IsInSight(obj))
            {
                lastSeenTransform.position = obj.transform.position;
                _objects.Add(obj);
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);

        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        destination.y = origin.y;

        if (Physics.Linecast(origin, destination, occlusionLayers))
        {
            return false;
        }
        
        return true;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];
        
        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;
        
        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;
        
        // Left Side
        
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;
        
        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;
        
        // Right Side
        
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;
        
        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;

        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;
            
            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;
            
            // Far Side
        
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;
        
            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;
        
            // Top Side
        
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // Bottom Side
        
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
            
            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        _mesh = CreateWedgeMesh();
        _scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (_mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
        }

        if (lastSeenTransform)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastSeenTransform.position, 0.2f);
        }
        
        
        // Gizmos.DrawWireSphere(transform.position, distance);
        //
        // for (int i = 0; i < _count; i++)
        // {
        //     Gizmos.DrawSphere(_colliders[i].transform.position, 0.2f);
        // }

        Gizmos.color = Color.red;
        foreach (var obj in _objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}
