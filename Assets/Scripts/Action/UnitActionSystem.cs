using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler OnBusyChanged;
    public event EventHandler OnActionStarted;

    private Unit selectedUnit;
    private BaseAction selectedAction;

    [SerializeField] private LayerMask unitsLayerMask;

    private bool isBusy;

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
        if (isBusy)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (TryHandleUnitSelection())
        {
            return;
        }
        if (!selectedUnit || !selectedAction)
        {
            return;
        }
        HandleSelectedAction();
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitsLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPositionInLevelGrid(MouseWorld.GetMousePosition());
            if (!selectedUnit.EnoughActionPointsToPerformAction(selectedAction))
            {
                return;
            }
            if (selectedAction.IsValidGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectedUnit.SpendActionPoints(selectedAction);
                selectedAction.PerformAction(mouseGridPosition, ClearBusy);
                OnActionStarted?.Invoke(this, EventArgs.Empty);
            }
        }
    }


    public void SetSelectedUnit(Unit unit)
    {
        this.selectedUnit = unit;
        this.selectedAction = null;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return this.selectedUnit;   
    }

    public void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool GetBusy()
    {
        return isBusy;
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        this.selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public BaseAction GetSelectedAction()
    {
        return this.selectedAction;
    }
}