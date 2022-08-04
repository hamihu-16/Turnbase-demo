using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private Animator unitAnimator;

    private float rotateSpeed = 10f;
    private int shootRange = 2;
    private float hitDamage = 40f;

    private Unit targetUnit;

    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }


    protected override void Awake()
    {
        base.Awake();
        unitAnimator = GetComponentInChildren<Animator>();
        actionCost = 2;
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        if (targetUnit == null)
        {
            return;
        }
        HandleAction();
    }

    public override void PerformAction(GridPosition inputGridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(inputGridPosition);
   
        
    }

    public void HandleAction()
    {      
        Vector3 shootDirection = (targetUnit.GetWorldPosition() - transform.position).normalized;
        transform.forward = shootDirection;
        Shoot();
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        targetUnit.TakeDamage(hitDamage);
        Debug.Log(targetUnit.transform.name + " dmged");
        ActionComplete();
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int i = -shootRange; i <= shootRange; i++)
        {
            for (int j = -shootRange; j <= shootRange; j++)
            {
                GridPosition offsetGridPosition = new GridPosition(i, j);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition)
                    || unitGridPosition == testGridPosition
                    || !LevelGrid.Instance.IsGridOccupied(testGridPosition))
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (unit.IsEnemy() == targetUnit.IsEnemy())
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
        return "Shoot";
    }

    public override int GetActionCost()
    {
        return this.actionCost;
    }
}
