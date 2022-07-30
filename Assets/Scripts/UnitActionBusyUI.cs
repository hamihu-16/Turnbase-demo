using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionBusyUI : MonoBehaviour
{
    [SerializeField] private GameObject unitActionBusyUI;

    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
    }

    private void UpdateActionBusy()
    {
        if (UnitActionSystem.Instance.GetBusy())
        {
            Debug.Log("setactive true");
            unitActionBusyUI.SetActive(true);
        }
        else
        {
            Debug.Log("setactive false");
            unitActionBusyUI.SetActive(false);
        }
    }
    private void UnitActionSystem_OnBusyChanged(object sender, EventArgs e)
    {
        Debug.Log("UnitActionSystem_OnBusyChanged" + UnitActionSystem.Instance.GetBusy());
        unitActionBusyUI.GetComponent<UnitActionBusyUI>().UpdateActionBusy();
    }
}
