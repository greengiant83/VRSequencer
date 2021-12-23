using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorBlock : MonoBehaviour
{
    [SerializeField] private Renderer Renderer;

    public DataCell Data { get; private set; }
    public Sequencer ParentSequencer { get; set; }

    private List<Collider> sensedItems = new List<Collider>();

    public void SetMaterial(Material Material)
    {
        Renderer.sharedMaterial = Material;
    }

    public void SetSize(Vector3 Size)
    {
        transform.localScale = Size * 0.95f;
    }

    public void SetData(DataCell Data)
    {
        this.Data = Data;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sensedItems.Contains(other)) return;

        sensedItems.Add(other);
        ParentSequencer.SensedItemEnter(Data, Vector3.zero);
    }

    private void OnTriggerExit(Collider other)
    {
        if(sensedItems.Contains(other))
        {
            sensedItems.Remove(other);
            ParentSequencer.SensedItemExit(Data);
        }
    }
}
