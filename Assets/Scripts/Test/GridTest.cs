using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            unit.GetMoveAction().GetValidGridPositionList();
        }
    }
}
