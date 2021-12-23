using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using AudioHelm;

public class Sequencer : ButtonInteractable
{
    [SerializeField] private Transform ColumnContainer;
    [SerializeField] private GameObject ColumnPrefab;
    [SerializeField] private BoxCollider Collider;
    [SerializeField] private Material BlockMaterialReference;

    public bool IsSpawning;
    public DataGrid Data { get; private set; }
    public float VolumeLow { get; private set; } = 0.0f;
    public float VolumeHigh { get; private set; } = 1.0f;
    public float BendWindow { get; set; }

    public bool IsMoving => (IsSpawning || grabAndSize.IsGrabbing);
    public Material BlockMaterial { get; private set; }

    private GrabAndSize grabAndSize;
    private Column[] columns;
    private Vector3 size;
    private HelmController audioController;

    protected override void Awake()
    {
        base.Awake();

        audioController = FindObjectOfType<HelmController>();
        BlockMaterial = Instantiate(BlockMaterialReference);
        SetVolumeEnvelope(VolumeLow, VolumeHigh);
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
        SequencerControlPanel.Instance.Show(this);
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

    public void SensedItemEnter(DataCell cell, Vector3 position)
    {
        if (cell.Index < 0) return;
        audioController.NoteOn(cell.Index + 50, 1.0f);
    }

    public void SensedItemExit(DataCell cell)
    {
        if (cell.Index < 0) return;
        audioController.NoteOff(cell.Index + 50);
    }

    public void SetVolumeEnvelope(float LowValue, float HighValue)
    {
        this.VolumeLow = LowValue;
        this.VolumeHigh = HighValue;
        BlockMaterial.SetFloat("_VolumeLow", LowValue);
        BlockMaterial.SetFloat("_VolumeHigh", HighValue);
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
            column.ParentSequencer = this;
            column.SetMaterial(BlockMaterial);
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
