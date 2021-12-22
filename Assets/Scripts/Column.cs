using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField] private GameObject SensorBlockPrefab;

    public DataColumn Data { get; private set; }

    private Vector3 size;
    private SensorBlock[] cells;
    private Material material;

    public void SetMaterial(Material Material)
    {
        this.material = Material;
    }

    public void SetSize(Vector3 Size)
    {
        this.size = Size;
        UpdateLayout();
    }

    public void SetData(DataColumn Data)
    {
        this.Data = Data;
        createLayout();
    }

    private void createLayout()
    {
        transform.ClearChildren();

        var activeCells = Data.Cells.Where(i => i.Value == true).ToArray();
        if(activeCells.Length == 0)
        {
            activeCells = new DataCell[1]
            {
                new DataCell()
                {
                    ParentColumn = Data,
                    Index = -1,
                    Value = false
                }
            };
        }

        cells = new SensorBlock[activeCells.Length];
        for(int i=0;i<activeCells.Length;i++)
        {
            var cell = Instantiate(SensorBlockPrefab).GetComponent<SensorBlock>();
            cell.transform.SetParent(this.transform, false);
            cell.SetData(activeCells[i]);
            cell.SetMaterial(material);
            cells[i] = cell;
        }

        UpdateLayout();
    }

    public void UpdateLayout()
    {
        var rowSize = new Vector3(size.x, size.y / (cells.Length), size.z);
        var position = new Vector3(0, -size.y / 2 + rowSize.y / 2, 0);
        foreach(var cell in cells)
        {
            cell.transform.localPosition = position;
            cell.SetSize(rowSize);
            position.y += rowSize.y;
        }
    }

    //private void createLayout()
    //{
    //    ColumnContainer.ClearChildren();

    //    var activeDataColumns = Data.Columns.Where(i => i.HasActiveCells()).ToArray();
    //    if (activeDataColumns.Length == 0)
    //    {
    //        activeDataColumns = new DataColumn[1]
    //        {
    //            new DataColumn()
    //            {
    //                ParentGrid = Data,
    //                Index = -1,
    //            }
    //        };
    //    }

    //    columns = new Column[activeDataColumns.Length];
    //    for (int i = 0; i < activeDataColumns.Length; i++)
    //    {
    //        var column = Instantiate(ColumnPrefab).GetComponent<Column>();
    //        column.transform.SetParent(ColumnContainer, false);
    //        column.SetData(activeDataColumns[i]);
    //        columns[i] = column;
    //    }

    //    updateLayout();
    //}

    //private void updateLayout()
    //{
    //    var colSize = new Vector3(size.x / (columns.Length), size.y, size.z);
    //    var position = new Vector3(-size.x / 2 + colSize.x / 2, 0, 0);
    //    foreach (var column in columns)
    //    {
    //        column.transform.localPosition = position;
    //        column.SetSize(colSize);
    //        position.x += colSize.x;
    //    }
    //}
}
