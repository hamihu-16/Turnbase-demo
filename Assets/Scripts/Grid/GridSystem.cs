using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjectArray;

    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridObjectArray = new GridObject[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GridPosition gridPosition = new GridPosition(i, j);
                gridObjectArray[i, j] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GridToWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public GridPosition WorldToGridPosition(Vector3 worldPosition) 
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize));
    }

    public void CreateDebugTextPrefabs (Transform textPrefab)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GridPosition gridPosition = new GridPosition(i, j);
                Transform textPrefabTransform = Transform.Instantiate(textPrefab, GridToWorldPosition(gridPosition), Quaternion.identity);
                GridObjectDebug gridObjectDebug = textPrefabTransform.GetComponent<GridObjectDebug>();
                gridObjectDebug.SetGridObject(GetGridObjectFromGridPosition(gridPosition));
            }
        }
    }

    public GridObject GetGridObjectFromGridPosition(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.z >= 0 &&
               gridPosition.x < width &&
               gridPosition.z < height;
    }

    public int GetWidth()
    {
        return this.width;
    }

    public int GetHeight()
    {
        return this.height;
    }
}
