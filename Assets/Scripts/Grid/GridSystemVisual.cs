using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform gridSystemVisualSingleGridPrefab;

    private GridSystemVisualSingleGrid[,] gridSystemVisualSingleGridArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Two or more GridSystemVisual active!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        int levelGridWidth = LevelGrid.Instance.GetWidth();
        int levelGridHeight = LevelGrid.Instance.GetHeight();
        gridSystemVisualSingleGridArray = new GridSystemVisualSingleGrid[levelGridWidth, levelGridHeight];
        for (int i = 0; i < levelGridWidth; i++)
        {
            for (int j = 0; j < levelGridHeight; j++)
            {
                GridPosition gridPosition = new GridPosition(i, j);
                Transform gridSystemVisualSingleGridTransform = Instantiate(gridSystemVisualSingleGridPrefab, LevelGrid.Instance.GetWorldPositionInLevelGrid(gridPosition), Quaternion.identity);
                gridSystemVisualSingleGridArray[i, j] = gridSystemVisualSingleGridTransform.GetComponent<GridSystemVisualSingleGrid>();
            }
        }
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPosition()
    {
        // why not hide right after instantiate?
        for (int i = 0; i < gridSystemVisualSingleGridArray.GetLength(0); i++) 
        {
            for (int j = 0; j < gridSystemVisualSingleGridArray.GetLength(1); j++)
            {
                gridSystemVisualSingleGridArray[i, j].HideVisual();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleGridArray[gridPosition.x, gridPosition.z].ShowVisual();
        }
    }

    public void UpdateGridVisual()
    {
        HideAllGridPosition();
        if (UnitActionSystem.Instance.GetSelectedUnit())
        {
            if (UnitActionSystem.Instance.GetSelectedAction())
            {
                BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
                List<GridPosition> gridPositionList = selectedAction.GetValidGridPositionList();
                ShowGridPositionList(gridPositionList);
            }
        }
    }
}
