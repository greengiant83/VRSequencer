using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;

public class Sequencer : ButtonInteractable
{
    [SerializeField] private Transform ColumnContainer;
    [SerializeField] private GameObject ColumnPrefab;
    [SerializeField] private BoxCollider Collider;

    public bool IsSpawning;
    public DataGrid Data { get; private set; }

    public bool IsMoving => (IsSpawning || grabAndSize.IsGrabbing);

    private GrabAndSize grabAndSize;
    private Column[] columns;
    private Vector3 size;

    protected override void Awake()
    {
        base.Awake();
    
        size = transform.localScale;
        initializeDataGrid(InputGrid.RowCount, InputGrid.ColCount);
        createLayout();

        grabAndSize = GetComponent<GrabAndSize>();
    }

    private void initializeDataGrid(int rows, int cols)
    {
        Data = new DataGrid();
        Data.Columns = new DataColumn[cols];
        for (int i = 0; i < Data.Columns.Length; i++)
        {
            var col = Data.Columns[i] = new DataColumn();
            col.ParentGrid = Data;
            col.Index = i;
            col.Cells = new DataCell[rows];
            for (int j = 0; j < col.Cells.Length; j++)
            {
                var cell = col.Cells[j] = new DataCell();
                cell.ParentColumn = col;
                cell.Index = j;
                cell.Value = false;
            }
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        grabAndSize.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        grabAndSize.OnSelectExited(args);
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        Select();
    }

    public void Select()
    {
        InputGrid.Show(this);
    }

    public Vector3 GetSize()
    {
        return size;
    }

    public void SetSize(Vector3 Size)
    {
        this.size = Size.Absolute();
        Collider.size = size;
        updateLayout();
    }

    public void DataUpdated()
    {
        createLayout();
    }

    private void createLayout()
    {
        ColumnContainer.ClearChildren();

        var activeDataColumns = Data.Columns.Where(i => i.HasActiveCells()).ToArray();
        if(activeDataColumns.Length == 0)
        {
            activeDataColumns = new DataColumn[1]
            {
                new DataColumn() 
                { 
                    ParentGrid = Data,
                    Cells = new DataCell[0],
                    Index = -1,                    
                }
            };
        }
        
        columns = new Column[activeDataColumns.Length];
        for(int i=0;i<activeDataColumns.Length;i++)
        {
            var column = Instantiate(ColumnPrefab).GetComponent<Column>();
            column.transform.SetParent(ColumnContainer, false);
            column.SetData(activeDataColumns[i]);
            columns[i] = column;
        }

        updateLayout();
    }

    private void updateLayout()
    {
        var colSize = new Vector3(size.x / (columns.Length), size.y, size.z);
        var position = new Vector3(-size.x / 2 + colSize.x / 2, 0, 0);
        foreach (var column in columns)
        {
            column.transform.localPosition = position;
            column.SetSize(colSize);
            position.x += colSize.x;
        }
    }

}
