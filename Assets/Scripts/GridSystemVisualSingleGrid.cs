using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingleGrid : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void ShowVisual()
    {
        meshRenderer.enabled = true;
    }

    public void HideVisual()
    {
        meshRenderer.enabled = false;
    }
}
