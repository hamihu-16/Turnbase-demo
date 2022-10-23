using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : BaseAction
{
    private Animator unitAnimator;
    private BuffAnimationEventHandler buffAnimationEventHandler;

    private int healRange = 4;
    private float healAmount = -25f; // negative value to reuse TakeDamage() function

    private Unit targetUnit;

    public event EventHandler<OnHealEventArgs> OnHeal;

    public class OnHealEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit actionUnit;
    }

    protected override void Awake()
    {
        base.Awake();
        unitAnimator = GetComponentInChildren<Animator>();
        actionCost = 1;

        buffAnimationEventHandler = GetComponentInChildren<BuffAnimationEventHandler>();
        buffAnimationEventHandler.OnBuffFired += BuffAnimationEventHandler_OnBuffFired;
        buffAnimationEventHandler.OnBuffComplete += BuffAnimationEventHandler_OnBuffComplete;
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
        Heal();
    }

    public void HandleAction()
    {
        Vector3 shootDirection = (targetUnit.GetWorldPosition() - transform.position).normalized;
        transform.forward = shootDirection;
    }

    private void Heal()
    {
        OnHeal?.Invoke(this, new OnHealEventArgs
        {
            targetUnit = targetUnit,
            actionUnit = unit
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
        for (int i = -healRange; i <= healRange; i++)
        {
            for (int j = -healRange; j <= healRange; j++)
            {
                GridPosition offsetGridPosition = new GridPosition(i, j);
                GridPosition checkGridPosition = unitGridPosition + offsetGridPosition;
                // grid is outside of level
                if (!LevelGrid.Instance.isValidGridPosition(checkGridPosition))
                {
                    continue;
                }

                /*// checkGrid is the grid the unit is standing on
                if (unitGridPosition == checkGridPosition)
                {
                    continue;
                }*/

                // if the grid has no unit on it
                if (!LevelGrid.Instance.IsGridOccupied(checkGridPosition))
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(checkGridPosition);
                // target is on different team
                if (unit.IsEnemy() != targetUnit.IsEnemy())
                {
                    continue;
                }

                /*if (targetUnit.GetHealth() >= 0)
                {
                    continue;
                }*/

                validGridPositionList.Add(checkGridPosition);
            }
        }
        return validGridPositionList;
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

    private void BuffAnimationEventHandler_OnBuffFired(object sender, EventArgs e)
    {
        targetUnit.TakeDamage(healAmount);
    }

    private void BuffAnimationEventHandler_OnBuffComplete(object sender, EventArgs e)
    {
        ActionComplete();
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidGridPositionList().Count;
    }

    public override string GetActionName()
    {
        return "Heal";
    }

    public override int GetActionCost()
    {
        return this.actionCost;
    }

    public int GetShootRange()
    {
        return this.healRange;
    }

    public float GetDamage()
    {
        return this.healAmount;
    }
}
