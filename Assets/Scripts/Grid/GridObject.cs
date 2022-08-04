using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject { 
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        this.unitList = new List<Unit>();
    }

    public List<Unit> GetUnitListInGridObject()
    {
        return this.unitList;
    }

    public void AddUnitInGridObject(Unit unit)
    {
        this.unitList.Add(unit);
    }

    public void RemoveUnitInGridObject(Unit unit)
    {
        this.unitList.Remove(unit);
    }

    public bool HasUnitInGridObject()
    {
        return unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if (HasUnitInGridObject())
        {
            return unitList[0];
        }
        return null;
    }

    public override string ToString()
    {
        string unitListString = "list count: " + unitList.Count + " ";
        foreach (Unit unit in unitList)
        {
            unitListString += unit + "\n";
        }
        return gridPosition.ToString() + "\n" + unitListString;
    }
}
