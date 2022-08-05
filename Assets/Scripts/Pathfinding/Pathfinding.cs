using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    [SerializeField] private Transform gridObjectTextPrefab;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    private GridSystem<PathNode> gridSystem;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Two or more Pathfinding active!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugTextPrefabs(gridObjectTextPrefab);
    }

    public List<GridPosition> FindShortestPath(GridPosition start, GridPosition end)
    {
        // init open and close lists
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObjectFromGridPosition(start);
        PathNode endNode = gridSystem.GetGridObjectFromGridPosition(end);
        
        openList.Add(startNode);

        InitPathNodes();

        while (openList.Count > 0)
        {
            //get lowest f cost node q from open list
            PathNode currentPathNode = GetLowestFCostPathNodeFromList(openList);

            //remove from open list
            openList.Remove(currentPathNode);
            closedList.Add(currentPathNode);
            
            //if q = end then stop
            if (currentPathNode == endNode)
            {
                return RetracePath(endNode, startNode);
            }

            //generate 8 surrounding nodes
            List<PathNode> surroundPathNodeList = GenerateSurroundPathNodes(currentPathNode);

            // calculate g, h, f 
            foreach (PathNode surroundPathNode in surroundPathNodeList)
            {
                if (closedList.Contains(surroundPathNode))
                {
                    continue;
                }

                int newGCost = currentPathNode.GetGCost() + CalculateDistance(currentPathNode, surroundPathNode);
                if (newGCost < surroundPathNode.GetGCost() || !openList.Contains(surroundPathNode))
                {
                    surroundPathNode.SetGCost(CalculateDistance(currentPathNode, surroundPathNode));
                    surroundPathNode.SetHCost(CalculateDistance(surroundPathNode, endNode));
                    surroundPathNode.CalculateFCost();
                    surroundPathNode.SetParentPathNode(currentPathNode);

                    if (!openList.Contains(surroundPathNode))
                    {
                        openList.Add(surroundPathNode);
                    }
                }
            }
        }
        return null;
    }

    private PathNode GetPathNode(int x, int z)
    {
        return gridSystem.GetGridObjectFromGridPosition(new GridPosition(x, z));
    }

    private void InitPathNodes()
    {
        for (int i = 0; i < gridSystem.GetWidth(); i++)
        {
            for (int j = 0; j < gridSystem.GetHeight(); j++)
            {
                GridPosition gridPosition = new GridPosition(i, j);
                PathNode pathNode = gridSystem.GetGridObjectFromGridPosition(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.SetParentPathNode(null);
            }
        }
    }

    private PathNode GetLowestFCostPathNodeFromList(List<PathNode> openList)
    {
        int lowestFCost = int.MaxValue;
        PathNode lowestFCostPathNode = null;
        foreach (PathNode pathNode in openList)
        {
            int pathNodeFCost = pathNode.GetFCost();
            if (pathNodeFCost < lowestFCost)
            {
                lowestFCost = pathNodeFCost;
                lowestFCostPathNode = pathNode;
            }
        }
        return lowestFCostPathNode;
    }

    private List<PathNode> GenerateSurroundPathNodes(PathNode pathNode)
    {
        List<PathNode> surroundingPathNodes = new List<PathNode>();
        GridPosition currentGridPosition = pathNode.GetGridPosition();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int checkX = currentGridPosition.x + i;
                int checkZ = currentGridPosition.z + j;

                if (checkX >= 0 && checkX < width && checkZ >= 0 && checkZ < height)
                {
                    surroundingPathNodes.Add(GetPathNode(checkX, checkZ));
                }
            }
        }
        return surroundingPathNodes;
    }

    public int CalculateDistance(PathNode start, PathNode end)
    {
        GridPosition startGridPosition = start.GetGridPosition();
        GridPosition endGridPosition = end.GetGridPosition();

        int distanceX = Mathf.Abs(endGridPosition.x - startGridPosition.x);
        int distanceZ = Mathf.Abs(endGridPosition.z - startGridPosition.z);

        if (distanceX > distanceZ)
            return MOVE_DIAGONAL_COST * distanceZ + MOVE_STRAIGHT_COST * (distanceX - distanceZ);
        return MOVE_DIAGONAL_COST * distanceX + MOVE_DIAGONAL_COST * (distanceZ - distanceX);
    }

    private List<GridPosition> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            pathNodeList.Add(currentNode);
            currentNode = currentNode.getParentPathNode();
        }
        pathNodeList.Reverse();

        List<GridPosition> path = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            path.Add(pathNode.GetGridPosition());
        }
        return path;
    }

    /*public void UpdateDuplicate(PathNode pathNode, List<PathNode> pathNodeList)
    {
        if (pathNodeList.Contains(pathNode))
        {
            pathNodeList.FindIndex();
        }
    }*/
}
