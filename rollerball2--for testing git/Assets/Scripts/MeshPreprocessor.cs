using UnityEngine;
using UnityMeshSimplifier;

public class MeshPreprocessor : MonoBehaviour
{
    public float quality = 0.5f; // 0 = max simplify, 1 = no simplify

    void Start()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter != null && filter.sharedMesh != null)
        {
            MeshSimplifier simplifier = new MeshSimplifier();
            simplifier.Initialize(filter.sharedMesh);
            simplifier.SimplifyMesh(quality);

            filter.sharedMesh = simplifier.ToMesh();
            filter.sharedMesh.RecalculateNormals();
            filter.sharedMesh.RecalculateBounds();
        }
    }
}
