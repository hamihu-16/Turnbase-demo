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
        HandleMove();
    }

    public void SetMovePosition(GridPosition inputGridPosition)
    {
        this.movePosition = LevelGrid.Instance.GetWorldPositionInLevelGrid(inputGridPosition);
        isActive = true;
    }

    public void HandleMove()
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
            isActive = false;
            unitAnimator.SetBool("IsMoving", false);
        }
    }

    public List<GridPosition> GetValidGridPositionList()
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

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return GetValidGridPositionList().Contains(gridPosition);
    }
}
