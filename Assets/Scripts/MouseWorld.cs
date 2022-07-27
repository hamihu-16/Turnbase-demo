using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld thisInstance;

    [SerializeField] private LayerMask planeLayerMask;

    private void Awake()
    {
        thisInstance = this;
    }

    public static Vector3 GetMousePosition()
    {
        Ray rayFromCamera = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(rayFromCamera, out RaycastHit hitInfo, float.MaxValue, thisInstance.planeLayerMask);
        return hitInfo.point;
    }
}
