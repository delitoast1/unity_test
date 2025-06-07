using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BladeTrail : MonoBehaviour
{
    public Transform tip;
    public Transform basePoint;

    public int trailFrameLength = 10;
    private const int NUM_VERTICES = 12;

    private Mesh trailMesh;
    private Vector3[] _vertices;
    private int[] _triangles;

    private int _frameCount = 0;
    private Vector3 _previousTipPosition;
    private Vector3 _previousBasePosition;

    private void Start()
    {
        trailMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = trailMesh;

        int maxVertices = trailFrameLength * NUM_VERTICES;
        _vertices = new Vector3[maxVertices];
        _triangles = new int[maxVertices];

        _previousTipPosition = tip.position;
        _previousBasePosition = basePoint.position;
    }

    private void LateUpdate()
    {
        if (_frameCount == trailFrameLength * NUM_VERTICES)
        {
            _frameCount = 0;
            trailMesh.Clear(); // reset mesh
        }

        // Current world positions
        Vector3 currTip = tip.position;
        Vector3 currBase = basePoint.position;

        // Convert to local space
        Vector3 localTip = transform.InverseTransformPoint(currTip);
        Vector3 localBase = transform.InverseTransformPoint(currBase);
        Vector3 localPrevTip = transform.InverseTransformPoint(_previousTipPosition);
        Vector3 localPrevBase = transform.InverseTransformPoint(_previousBasePosition);

        // === Your Triangle Geometry ===
        _vertices[_frameCount] = localBase;
        _vertices[_frameCount + 1] = localTip;
        _vertices[_frameCount + 2] = localPrevTip;
        _vertices[_frameCount + 3] = localBase;
        _vertices[_frameCount + 4] = localPrevTip;
        _vertices[_frameCount + 5] = localTip;

        _vertices[_frameCount + 6] = localPrevTip;
        _vertices[_frameCount + 7] = localBase;
        _vertices[_frameCount + 8] = localPrevBase;
        _vertices[_frameCount + 9] = localPrevTip;
        _vertices[_frameCount + 10] = localPrevBase;
        _vertices[_frameCount + 11] = localBase;

        _triangles[_frameCount] = _frameCount;
        _triangles[_frameCount + 1] = _frameCount + 1;
        _triangles[_frameCount + 2] = _frameCount + 2;
        _triangles[_frameCount + 3] = _frameCount + 3;
        _triangles[_frameCount + 4] = _frameCount + 4;
        _triangles[_frameCount + 5] = _frameCount + 5;
        _triangles[_frameCount + 6] = _frameCount + 6;
        _triangles[_frameCount + 7] = _frameCount + 7;
        _triangles[_frameCount + 8] = _frameCount + 8;
        _triangles[_frameCount + 9] = _frameCount + 9;
        _triangles[_frameCount + 10] = _frameCount + 10;
        _triangles[_frameCount + 11] = _frameCount + 11;

        // Assign mesh
        trailMesh.vertices = _vertices;
        trailMesh.triangles = _triangles;
        trailMesh.RecalculateNormals();

        // Update previous frame info
        _previousTipPosition = currTip;
        _previousBasePosition = currBase;
        _frameCount += NUM_VERTICES;
    }
}
