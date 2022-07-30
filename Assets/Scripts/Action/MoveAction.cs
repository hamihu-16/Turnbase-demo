using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private Animator unitAnimator;

    private float rotateSpeed = 10f;
    private float moveSpeed = 5f;
    private float stoppingDistance = 0.1f;
    private int moveRange = 2;
    private Vector3 movePosition;


    protected override void Awake()
    {
        base.Awake();
        unitAnimator = GetComponentInChildren<Animator>();
        this.movePosition = transform.position;
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
        this.onActionComplete = onActionComplete;
        this.movePosition = LevelGrid.Instance.GetWorldPositionInLevelGrid(inputGridPosition);
        isActive = true;
    }

    public void HandleAction()
    {
        if (Vector3.Distance(transform.position, movePosition) >= stoppingDistance)
        {
            Vector3 moveDirection = (movePosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
            unitAnimator.SetBool("IsMoving", true);
        }
        else
        {
            unitAnimator.SetBool("IsMoving", false);
            isActive = false;
            onActionComplete();
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
}
