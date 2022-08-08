using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPositionInLevelGrid(MouseWorld.GetMousePosition());
            GridPosition startGridPosition = new GridPosition(0, 0);

            List<GridPosition> gridPositionList = Pathfinding.Instance.FindShortestPath(startGridPosition, mouseGridPosition);

            for (int i = 0; i < gridPositionList.Count - 1; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPositionInLevelGrid(gridPositionList[i]),
                    LevelGrid.Instance.GetWorldPositionInLevelGrid(gridPositionList[i + 1]),
                    Color.white,
                    100f
                );
            }
        }

    }
}
