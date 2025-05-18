using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BladeTrail : MonoBehaviour
{
    public Transform tip;
    public Transform basePoint;
    public Transform player;

    [Tooltip("Number of trail frames to store")]
    public int frameCount = 3;

    private const int VERTICES_PER_FRAME = 18;

    private Mesh trailMesh;
    private Vector3[] vertices;
    private int[] triangles;
    private int currentIndex = 0;

    private Vector3 prevTip;
    private Vector3 prevBase;

    private void Start()
    {
        this.transform.position = player.position;
        trailMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = trailMesh;

        int maxVertices = frameCount * VERTICES_PER_FRAME;
        vertices = new Vector3[maxVertices];
        triangles = new int[maxVertices];

        prevTip = tip.position;
        prevBase = basePoint.position;
    }

    private void LateUpdate()
    {
        if (currentIndex + VERTICES_PER_FRAME > vertices.Length)
            currentIndex = 0;

        Vector3 currTip = tip.position;
        Vector3 currBase = basePoint.position;

        // Draw in local space relative to TrailMesh's transform
        currTip = transform.InverseTransformPoint(currTip);
        currBase = transform.InverseTransformPoint(currBase);
        Vector3 localPrevTip = transform.InverseTransformPoint(prevTip);
        Vector3 localPrevBase = transform.InverseTransformPoint(prevBase);

        // Triangle 1: tip sweep
        vertices[currentIndex + 0] = currBase;
        vertices[currentIndex + 1] = currTip;
        vertices[currentIndex + 2] = localPrevTip;

        vertices[currentIndex + 3] = currBase;
        vertices[currentIndex + 4] = localPrevTip;
        vertices[currentIndex + 5] = currTip;

        // Triangle 2: base sweep
        vertices[currentIndex + 6] = localPrevTip;
        vertices[currentIndex + 7] = currBase;
        vertices[currentIndex + 8] = localPrevBase;

        vertices[currentIndex + 9] = localPrevTip;
        vertices[currentIndex + 10] = localPrevBase;
        vertices[currentIndex + 11] = currBase;

        for (int i = 0; i < VERTICES_PER_FRAME; i++)
            triangles[currentIndex + i] = currentIndex + i;

        trailMesh.Clear();
        trailMesh.vertices = vertices;
        trailMesh.triangles = triangles;
        trailMesh.RecalculateNormals();

        prevTip = tip.position;
        prevBase = basePoint.position;
        currentIndex += VERTICES_PER_FRAME;
    }
}
