using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MoveAction : BaseAction
{
    private float rotateSpeed = 10f;
    private float moveSpeed = 4f;
    private float stoppingDistance = 0.2f;
    [SerializeField] private int moveRange = 4;

    private List<Vector3> positionList;
    private int currentPositionIndex;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

/*    protected override void Awake()
    {
        base.Awake();
        this.movePosition = transform.position;
        actionCost = 1;
    }*/

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        HandleAction();
    }

    public override void PerformAction(GridPosition inputGridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindShortestPath(unit.GetGridPosition(), inputGridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPositionInLevelGrid(pathGridPosition));
        }

        ActionStart(onActionComplete);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        Debug.Log("OnStartMoving");
        Debug.Log(positionList[positionList.Count - 1]);
    }

    public void HandleAction()
    {
        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                Debug.Log("OnStopMoving");
            }
        }
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int i = -moveRange; i <= moveRange; i++)
        {
            for (int j = -moveRange; j <= moveRange; j++)
            {
                GridPosition offsetGridPosition = new GridPosition(i, j);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition)
                    || unitGridPosition == testGridPosition
                    || LevelGrid.Instance.IsGridOccupied(testGridPosition))
                {
                    continue;
                }
                if (!Pathfinding.Instance.IsGridPositionWalkable(testGridPosition))
                {
                    continue;
                }

                int pathLength;
                List<GridPosition> path = Pathfinding.Instance.FindShortestPath(unitGridPosition, testGridPosition, out pathLength);
                int pathfindingDistanceMultiplier = 10;

                if (path == null)
                {
                    continue;
                }
                if (pathLength > moveRange * pathfindingDistanceMultiplier)
                {
                    // Path length is too long
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override int GetActionCost()
    {
        return this.actionCost;
    }

    public int GetMoveRange()
    {
        return this.moveRange;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition;
        if (unit.GetShootAction() != null)
        {
            targetCountAtGridPosition = unit.GetShootAction().GetTargetCountAtPosition(gridPosition);
        }
        else
        {
            if (gridPosition.x == 3)
            {
                Debug.Log("");
            }
            targetCountAtGridPosition = unit.GetSwordAction().GetTargetCountAtPosition(gridPosition);            
        }


        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }

}
