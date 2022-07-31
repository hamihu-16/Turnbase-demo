using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int MAX_ACTION_POINTS = 2;

    private GridPosition gridPosition;
    private MoveAction moveAction;
    private BaseAction[] baseActionArray;
    private int actionPoints = MAX_ACTION_POINTS;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPositionInLevelGrid(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnEnd += TurnSystem_OnTurnEnd;
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

    public MoveAction GetMoveAction()
    {
        return this.moveAction;
    }

    public GridPosition GetGridPosition()
    {
        return this.gridPosition;
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

    public void TurnSystem_OnTurnEnd(object sender, EventArgs e)
    {
        this.actionPoints = MAX_ACTION_POINTS;
    }
}
