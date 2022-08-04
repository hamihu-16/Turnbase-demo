using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private float rotateSpeed = 10f;
    private float moveSpeed = 5f;
    private float stoppingDistance = 0.1f;
    private int moveRange = 2;
    private Vector3 movePosition;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    protected override void Awake()
    {
        base.Awake();
        this.movePosition = transform.position;
        actionCost = 1;
    }

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
        ActionStart(onActionComplete);
        this.movePosition = LevelGrid.Instance.GetWorldPositionInLevelGrid(inputGridPosition);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
    }

    public void HandleAction()
    {
        if (Vector3.Distance(transform.position, movePosition) >= stoppingDistance)
        {
            Vector3 moveDirection = (movePosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        }
        else
        {
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
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
}
