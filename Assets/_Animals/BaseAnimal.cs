using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseAnimal : MonoBehaviour {
    public string UnitName;
    public Tile OccupiedTile;
    public Vector2 Location;

    public virtual Vector2 MoveRandom() {
        Tile destinationTile = GridManager.Instance.GetRandomPosition();
        Console.WriteLine(destinationTile);
        OccupiedTile = destinationTile;
        return destinationTile.transform.position;
    }
    private List<Vector2> ShortestPath() {
        //A* Algorithm path
        Vector2 start = Location;
        Vector2 finish = MoveRandom();
        Vector2 currentTile = start;

        List<Vector2> openList = new List<Vector2>();
        List<Vector2> closedList = new List<Vector2>();

        List<Vector2> RoutePath = new List<Vector2>();

        //Start by adding original position to the open list
        openList.Add(start);
        Vector2 parent;
        while (RoutePath.Contains(finish) == false) { //RoutePath or closedlist?
             while (openList.Count > 0) {
            //get the square with the lowest F score: 
            //distance from finish + distance from start
            Vector2 lowest = openList[0];
            foreach (Vector2 vector in openList) { //Could use a mapping function
                double fval = GetScores(vector, start, finish)[2];
                if (fval <= GetScores(lowest, start, finish)[2]) {
                    lowest = vector;
                }
            }

            if (lowest == finish) {
                break;
            } else {
                closedList.Add(lowest);
                Vector2[] adjacent_diffs = {new Vector2(1,0), new Vector2(1,1), new Vector2(0,1), new Vector2(-1,0), new Vector2(-1,-1), new Vector2(0,-1)}; //Need to check the coordinates point to a real tile
                foreach (Vector2 diff in adjacent_diffs){
                    Vector2 neighbour = diff + currentTile;
                    double[] fScores_current = GetScores(currentTile, start, finish);
                    if (GridManager.Instance.GetTileAtPosition(neighbour) != null && GridManager.Instance.GetTileAtPosition(neighbour).Walkable) {
                           double[]ghfScore_neighbour = GetScores(neighbour, start, finish);
                           if (closedList.Contains(neighbour)) {
                            continue; //Go to next adjacent square
                           } 
                           else if (!openList.Contains(neighbour)) {
                            //Compute its score, set the parent
                            openList.Add(neighbour);
                            parent = currentTile;
                           } 
                           else {
                             //If it's already in the open list
                             //Test if using current G score makes the neighbour F score lower, if yes update the parent as it is a better path
                             if (ghfScore_neighbour[2] < fScores_current[2]) {
                                parent = currentTile;
                                
                             }
                             return closedList;
                           }
                            
                        
                    }
                }

                }

            }
        }
        return null;
        }

    private double[] GetScores(Vector2 coords, Vector2 start, Vector2 finish) {
        double g_score = Math.Pow((double)(Math.Sqrt((coords.x - finish.x) + (coords.y - finish.y))), 2.00);
        double h_score = Math.Pow((double)(Math.Sqrt((coords.x - start.x) + (coords.y - start.y))), 2.00);
        double[] ghfScores = {g_score, h_score, g_score + h_score};
        return ghfScores;
    }

}