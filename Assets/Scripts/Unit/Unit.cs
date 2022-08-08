using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected const int MAX_ACTION_POINTS = 2;

    protected GridPosition gridPosition;

    protected HealthSystem healthSystem;
    protected BaseAction[] baseActionArray;


    protected int actionPoints = MAX_ACTION_POINTS;

    [SerializeField] protected bool isEnemy;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPositionInLevelGrid(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnEnd += TurnSystem_OnTurnEnd;
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void Update()
    {
        HandleUnitGridPosition();
    }

    public void HandleUnitGridPosition()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPositionInLevelGrid(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UpdateUnitGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public T GetAction<T>() where T : BaseAction
    { 
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }


    public GridPosition GetGridPosition()
    {
        return this.gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return this.baseActionArray;
    }

    public int GetActionPoints()
    {
        return this.actionPoints;
    }

    public bool EnoughActionPointsToPerformAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionCost())
        {
            return true;
        }
        return false;
    }

    public void SpendActionPoints(BaseAction baseAction)
    {
        this.actionPoints -= baseAction.GetActionCost();
    }
    public bool IsEnemy()
    {
        return this.isEnemy;
    }

    public void TakeDamage(float damage)
    {
        healthSystem.TakeDamage(damage);
    }

    public void TurnSystem_OnTurnEnd(object sender, EventArgs e)
    {
        if ((isEnemy && !TurnSystem.Instance.IsPlayerTurn()) 
            || (!isEnemy && TurnSystem.Instance.IsPlayerTurn()))
        {
            this.actionPoints = MAX_ACTION_POINTS;
        }
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
    }

}
