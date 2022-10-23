using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private Animator unitAnimator;
    private RangedAnimationEventHandler rangedAnimationEventHandler;

    private int shootRange = 4;
    private float hitDamage = 40f;

    [SerializeField] private LayerMask obstaclesLayerMask;
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
        actionCost = 1;

        rangedAnimationEventHandler = GetComponentInChildren<RangedAnimationEventHandler>();
        rangedAnimationEventHandler.OnProjectileFired += RangedAnimationEventHandler_OnProjectileFired;
        rangedAnimationEventHandler.OnProjectileComplete += RangedAnimationEventHandler_OnProjectileComplete;
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
        Shoot();
    }

    public void HandleAction()
    {      
        Vector3 shootDirection = (targetUnit.GetWorldPosition() - transform.position).normalized;
        transform.forward = shootDirection;
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        for (int i = -shootRange; i <= shootRange; i++)
        {
            for (int j = -shootRange; j <= shootRange; j++)
            {
                GridPosition offsetGridPosition = new GridPosition(i, j);
                GridPosition checkGridPosition = unitGridPosition + offsetGridPosition;
                // grid is outside of level
                if (!LevelGrid.Instance.isValidGridPosition(checkGridPosition)) {
                    continue;
                }

                // checkGrid is the grid the unit is standing on
                if (unitGridPosition == checkGridPosition)
                {
                    continue;
                }

                // if the grid has no unit on it
                if (!LevelGrid.Instance.IsGridOccupied(checkGridPosition))
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(checkGridPosition);
                // target is on the same team
                if (unit.IsEnemy() == targetUnit.IsEnemy())
                {
                    continue;
                }

                if (targetUnit.GetHealth() <= 0)
                {
                    continue;
                }
                
                if (IsBlockedByObstacle(targetUnit))
                {
                    continue;
                }

                validGridPositionList.Add(checkGridPosition);
            }
        }
        return validGridPositionList;
    }

    public bool IsBlockedByObstacle(Unit targetUnit)
    {
        Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPositionInLevelGrid(unit.GetGridPosition());
        Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

        float unitShoulderHeight = 1.7f;
        if (Physics.Raycast(
                unitWorldPosition + Vector3.up * unitShoulderHeight,
                shootDirection,
                Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                obstaclesLayerMask))
        {
            // Blocked by an Obstacle
            return true;
        }

        return false;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt(100 - targetUnit.GetHealth()),
        };
    }

    private void RangedAnimationEventHandler_OnProjectileFired(object sender, EventArgs e)
    {
        targetUnit.TakeDamage(hitDamage);
    }

    private void RangedAnimationEventHandler_OnProjectileComplete(object sender, EventArgs e)
    {
        ActionComplete();
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidGridPositionList().Count;
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override int GetActionCost()
    {
        return this.actionCost;
    }

    public int GetShootRange()
    {
        return this.shootRange;
    }

    public float GetDamage()
    {
        return this.hitDamage;
    }

}
