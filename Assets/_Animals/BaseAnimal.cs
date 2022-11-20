using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BaseAnimal : MonoBehaviour
{
    public string UnitName;
    public Tile OccupiedTile;
    public Vector2 Location;
    void Awake()
    {
        Location = transform.position;
    }
    private class Node
    {
        public Vector2 position;
        public Node parent;
        public double gCost;
        public double hCost;
        public double fCost;
        public Node(Vector2 position, Node parent)
        { //Should implement an fVal 
            this.position = position;
            this.parent = parent;
        }
    }


    public List<Vector2> ShortestPath()
    {
        //A* Algorithm path
        Node start = new Node(Location, null);
        Node finish = new Node(GridManager.Instance.GetRandomPosition().transform.position, null);
        Node currentTile = start;
        Debug.Log(currentTile);

        List<Node> openList = new List<Node>();
        openList.Add(currentTile);
        List<Vector2> closedList = new List<Vector2>();

        //Start by adding original position to the open list
        while (!closedList.Contains(finish.position))
        {
            while (openList.Count > 0)
            {
                //get the square with the lowest F score: 
                //distance from finish + distance from start
                Node lowest = openList[0];
                foreach (Node adjNode in openList)
                { //Could use a mapping function
                    double fval = GetFCost(adjNode, start, finish);
                    if (fval <= GetFCost(lowest, start, finish))
                    {
                        lowest = adjNode; //Finding node in open with the smallest F-val
                    }
                }
                currentTile = lowest;

                if (lowest.Equals(finish))
                {
                    return CalculatePath(finish);
                }

                openList.Remove(currentTile);
                closedList.Add(currentTile.position);


                Vector2[] adjacent_diffs = { new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1) }; //Need to check the coordinates point to a real tile

                foreach (Vector2 diff in adjacent_diffs)
                {
                    Node neighbour = new Node(currentTile.position + diff, null);
                    if (closedList.Contains(neighbour.position))
                    {
                        continue; //Go to next adjacent square
                    }

                    double tentativeGCost = GetGCost(currentTile, start) + CalculateDistanceCost(currentTile, neighbour);

                    //If it's already in the open list
                    //Test if using current G score makes the neighbour F score lower, if yes update the parent as it is a better path
                    if (tentativeGCost < GetGCost(neighbour, start))
                    {
                        neighbour.parent = currentTile;
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = CalculateDistanceCost(neighbour, finish);
                        //Add a gcost property

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }

                    }
                }

            }
        }
        return null;
    }

    private double[] GetScores(Node currNode, Vector2 start, Vector2 finish)
    {
        double g_score = Math.Pow((double)(Math.Sqrt((currNode.position.x - finish.x) + (currNode.position.y - finish.y))), 2.00);
        double h_score = Math.Pow((double)(Math.Sqrt((currNode.position.x - start.x) + (currNode.position.y - start.y))), 2.00);
        double[] ghfScores = { g_score, h_score, g_score + h_score };
        return ghfScores;
    }

    private double CalculateDistanceCost(Node current, Node neighbour)
    {
        double distance = Math.Pow((double)(Math.Sqrt((current.position.x - neighbour.position.x) + (current.position.y - neighbour.position.y))), 2.00);
        return distance;
    }

    private List<Vector2> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while (currentNode.parent != null)
        {
            path.Add(currentNode.parent); ;
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path.Select(node => node.position).ToList();
    }

    private double GetGCost(Node node, Node start)
    {
        return (start.position - node.position).magnitude;

    }

    private double GetHCost(Node node, Node finish)
    {
        return (finish.position - node.position).magnitude;
    }

    private double GetFCost(Node node, Node start, Node finish)
    {
        return GetGCost(node, start) + GetHCost(node, finish);
    }

    // ! Code Monkey Section

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    private Dictionary<Vector2, PathNode> _GridPathNodes = new Dictionary<Vector2, PathNode>();

    public List<PathNode> FindPath()
    {
        PathNode startNode = new PathNode(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
        _GridPathNodes[new Vector2(transform.position.x, transform.position.y)] = startNode;
        var targetTile = GridManager.Instance.GetRandomPosition();
        PathNode endNode = new PathNode(Mathf.FloorToInt(targetTile.transform.position.x), Mathf.FloorToInt(targetTile.transform.position.y));
        _GridPathNodes[new Vector2(targetTile.transform.position.x, targetTile.transform.position.y)] = endNode;

        if (startNode == null || endNode == null)
        {
            // Invalid Path
            return null;
        }

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // Reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < GridManager.Instance.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < GridManager.Instance.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < GridManager.Instance.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < GridManager.Instance.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    public PathNode GetNode(int x, int y)
    {
        if (!_GridPathNodes.ContainsKey(new Vector2(x, y)))
        {
            var tile = GridManager.Instance.GetTileAtPosition(new Vector2(x, y));
            _GridPathNodes[new Vector2(x, y)] = new PathNode(x, y, tile.Walkable);
        }
        return _GridPathNodes[new Vector2(x, y)];
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}