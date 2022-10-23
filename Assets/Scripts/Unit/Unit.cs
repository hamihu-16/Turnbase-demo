using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    protected const int MAX_ACTION_POINTS = 2;

    protected GridPosition gridPosition;

    protected HealthSystem healthSystem;
    protected BaseAction[] baseActionArray;

    protected int actionPoints = MAX_ACTION_POINTS;
    public static EventHandler OnAnyActionPointsChanged;

    public static EventHandler OnAnyUnitSpawned;
    public static EventHandler OnAnyUnitDead;

    [SerializeField] private bool isEnemy;

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
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
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
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UpdateUnitGridPosition(this, oldGridPosition, newGridPosition);
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

    public bool IsEnemy()
    {
        return isEnemy;
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
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void TakeDamage(float damage)
    {
        healthSystem.TakeDamage(damage);
    }

    public void Heal(float healAmount)
    {
        healthSystem.TakeDamage(healAmount);
    }

    public float GetHealth()
    {
        return healthSystem.GetHealth();
    }

    public ShootAction GetShootAction()
    {
        return GetComponent<ShootAction>();
    }

    public SwordAction GetSwordAction()
    {
        return GetComponent<SwordAction>();
    }

    virtual public void TurnSystem_OnTurnEnd(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())
            || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            this.actionPoints = MAX_ACTION_POINTS;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

}
