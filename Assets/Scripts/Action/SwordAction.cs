using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SwordAction : BaseAction
{
    private Animator unitAnimator;
    private MeleeAnimationEventHandler meleeAnimationEventHandler;

    private int swordRange = 1;
    [SerializeField] private float hitDamage = 100f;

    private Unit targetUnit;

    public event EventHandler<OnSwordEventArgs> OnSword;

    public class OnSwordEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit unit;
    }

    protected override void Awake()
    {
        base.Awake();
        unitAnimator = GetComponentInChildren<Animator>();
        actionCost = 1;

        meleeAnimationEventHandler = GetComponentInChildren<MeleeAnimationEventHandler>();
        meleeAnimationEventHandler.OnMeleeHit += MeleeAnimationEventHandler_OnMeleeHit;
        meleeAnimationEventHandler.OnMeleeComplete += MeleeAnimationEventHandler_OnMeleeComplete;
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
        Sword();
    }

    public void HandleAction()
    {
        Vector3 shootDirection = (targetUnit.GetWorldPosition() - transform.position).normalized;
        transform.forward = shootDirection;
    }

    private void Sword()
    {
        OnSword?.Invoke(this, new OnSwordEventArgs
        {
            targetUnit = targetUnit,
            unit = unit
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
        for (int i = -swordRange; i <= swordRange; i++)
        {
            for (int j = -swordRange; j <= swordRange; j++)
            {
                GridPosition offsetGridPosition = new GridPosition(i, j);
                GridPosition checkGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.isValidGridPosition(checkGridPosition))
                {
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
            actionValue = 200,
        };
    }

    private void MeleeAnimationEventHandler_OnMeleeHit(object sender, EventArgs e)
    {
        targetUnit.TakeDamage(hitDamage);
    }

    private void MeleeAnimationEventHandler_OnMeleeComplete(object sender, EventArgs e)
    {
        ActionComplete();
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override int GetActionCost()
    {
        return this.actionCost;
    }

    public int GetShootRange()
    {
        return this.swordRange;
    }

    public float GetDamage()
    {
        return this.hitDamage;
    }

}
