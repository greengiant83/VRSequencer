using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorBlock : MonoBehaviour
{
    public DataCell Data { get; private set; }

    public void SetSize(Vector3 Size)
    {
        transform.localScale = Size * 0.95f;
    }

    public void SetData(DataCell Data)
    {
        this.Data = Data;
    }
}
