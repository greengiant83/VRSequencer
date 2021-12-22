using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataGrid
{
    public DataColumn[] Columns;
}

public class DataColumn
{
    public DataGrid ParentGrid { get; set; }
    public int Index { get; set; }
    public DataCell[] Cells { get; set; }

    public bool HasActiveCells()
    {
        return Cells.Any(i => i.Value == true);
    }
}

public class DataCell
{
    public DataColumn ParentColumn { get; set; }
    public int Index { get; set; }
    public bool Value { get; set; }
}
