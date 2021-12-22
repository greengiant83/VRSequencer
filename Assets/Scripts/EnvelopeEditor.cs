using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvelopeEditor : MonoBehaviour
{
    [SerializeField] private Transform Handle1;
    [SerializeField] private Transform Handle2;
    [SerializeField] private MeshFilter MeshFilter;

    private Mesh mesh;
    private Vector3[] verts;

    void Start()
    {
        MeshFilter.transform.localScale = Vector3.one;
        mesh = Instantiate(MeshFilter.sharedMesh);
        MeshFilter.sharedMesh = mesh;
        verts = mesh.vertices;
    }

    void Update()
    {
        verts[3] = Handle1.localPosition;
        verts[1] = Handle2.localPosition;
        mesh.vertices = verts;
    }
}
