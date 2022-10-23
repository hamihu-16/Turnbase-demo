using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEventManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyList1;
    [SerializeField] private List<GameObject> enemyList2;
    [SerializeField] private GameObject hiders;

    private void Start()
    {
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        if (enemyList1.Count == 0)
        {
            hiders.SetActive(false);
            SetActiveGameObject(enemyList2);
        }
    }

    private void SetActiveGameObject(List<GameObject> gameObjectList)
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
    }
}
