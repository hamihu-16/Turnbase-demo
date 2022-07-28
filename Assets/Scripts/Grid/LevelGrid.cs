using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [SerializeField] Transform textPrefab;
    private GridSystem gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Two or more UnitActionSystem active!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugTextPrefabs(textPrefab); 
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObjectFromGridPosition(gridPosition).AddUnitInGridObject(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObjectFromGridPosition(gridPosition).GetUnitListInGridObject();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObjectFromGridPosition(gridPosition).RemoveUnitInGridObject(unit);
    }

    public void UpdateUnitGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);

        AddUnitAtGridPosition(toGridPosition, unit);
    }

    public GridPosition GetGridPositionInLevelGrid(Vector3 worldPosition) => gridSystem.WorldToGridPosition(worldPosition);

    public Vector3 GetWorldPositionInLevelGrid(GridPosition gridPosition) => gridSystem.GridToWorldPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public bool isValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool IsGridOccupied(GridPosition gridPosition)
    {
        return gridSystem.GetGridObjectFromGridPosition(gridPosition).HasUnitInGridObject();
    }
}
