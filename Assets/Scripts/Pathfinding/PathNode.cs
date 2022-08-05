using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode parentPathNode;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public GridPosition GetGridPosition()
    {
        return this.gridPosition;
    }

    public int GetGCost()
    {
        return this.gCost;
    }

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }

    public int GetHCost()
    {
        return this.hCost;
    }

    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }

    public int GetFCost()
    {
        return this.fCost;
    }

    public void CalculateFCost()
    {
        this.fCost = gCost + hCost;
    }

    public PathNode getParentPathNode()
    {
        return this.parentPathNode;
    }

    public void SetParentPathNode(PathNode parentPathNode)
    {
        this.parentPathNode = parentPathNode;
    }
}
