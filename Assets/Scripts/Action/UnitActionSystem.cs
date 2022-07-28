using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;

    private Unit selectedUnit;
    [SerializeField] private LayerMask unitsLayerMask;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Two or more UnitActionSystem active!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) 
                return;
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPositionInLevelGrid(MouseWorld.GetMousePosition());
            if (selectedUnit.GetMoveAction().IsValidGridPosition(mouseGridPosition))
            {
                selectedUnit.GetMoveAction().SetMovePosition(mouseGridPosition);
            }
        }      
    }

    public bool TryHandleUnitSelection()
    {
        Ray rayFromCamera = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayFromCamera, out RaycastHit hitInfo, float.MaxValue, unitsLayerMask))
        {
            if (hitInfo.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    public void SetSelectedUnit(Unit unit)
    {
        this.selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return this.selectedUnit;   
    }
}