using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvelopeEditor : MonoBehaviour
{
    [SerializeField] private EnvelopeHandle LowHandle;
    [SerializeField] private EnvelopeHandle HighHandle;
    [SerializeField] private MeshFilter MeshFilter;

    public float LowValue;
    public float HighValue;

    private Mesh mesh;
    private Vector3[] verts;
    private Sequencer source;

    private void Start()
    {
        MeshFilter.transform.localScale = Vector3.one;
        mesh = Instantiate(MeshFilter.sharedMesh);
        MeshFilter.sharedMesh = mesh;
        verts = mesh.vertices;
    }

    private void Update()
    {
        if (LowHandle.IsGrabbing || HighHandle.IsGrabbing)
        {
            updateMesh();

            if (source != null)
            {
                LowValue = verts[1].y + 0.5f;
                HighValue = verts[3].y + 0.5f;
                source.SetVolumeEnvelope(LowValue, HighValue);
            }
        }
    }

    public void SetSource(Sequencer Source)
    {
        this.source = Source;
    }

    public void SetDepth(float Depth)
    {
        MeshFilter.transform.localScale = new Vector3(1, Depth, 1);

        setHandlePosition(LowHandle, source.VolumeLow);
        setHandlePosition(HighHandle, source.VolumeHigh);
        updateMesh();
    }

    public float GetHandleRange()
    {
        return 0.5f * MeshFilter.transform.localScale.y;
    }

    private void setHandlePosition(EnvelopeHandle handle, float value)
    {
        var position = handle.transform.localPosition;
        position.z = (value - 0.5f) * MeshFilter.transform.localScale.y;
        handle.transform.localPosition = position;
    }

    private void updateMesh()
    {
        verts[1] = MeshFilter.transform.InverseTransformPoint(LowHandle.transform.position);
        verts[3] = MeshFilter.transform.InverseTransformPoint(HighHandle.transform.position);
        mesh.vertices = verts;
    }
}
