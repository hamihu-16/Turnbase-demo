using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridObjectDebug : MonoBehaviour
{
    private object gridObject;
    private TextMeshPro textMeshPro;

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }
    protected virtual void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }
}
