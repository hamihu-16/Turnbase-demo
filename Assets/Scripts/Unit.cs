using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private MoveAction moveAction;
    

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPositionInLevelGrid(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        //HandleMove();
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
}
