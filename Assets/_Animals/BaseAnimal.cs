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
    }

    private List<Tile> ShortestPath() {
        //A* Algorithm path
        Vector2 start = Location;
        Vector2 finish = MoveRandom();
        Vector2 currentTile = start;

        List openList = new List<>();
        List closedList = new List<>();

        List RoutePath = new List<>();

        //Start by adding original position to the open list
        openList.Add(start);
        Vector2 parent = null;
        while (RoutePath.Contains(finish) == false) { //RoutePath or closedlist?
             while (openList.Count > 0) {
            //get the square with the lowest F score: 
            //distance from finish + distance from start
            var lowest = openList.Min(getLScore(Location, start, finish));
            if (lowest == finish) {
                break;
            } else {
                closedList.Add(lowest);
                Vector2[] neighbours = {Vector2.Add(currentTile, (1,0)), Vector2.Add(currentTile, (1,1)),Vector2.Add(locurrentTilewest, (0,1)),Vector2.Add(currentTile, (-1,0)),Vector2.Add(currentTile, (-1,-1)),Vector2.Add(currentTile, (0,-1))}; //Need to check the coordinates point to a real tile
                foreach (Vector2 neighbour in neighbours){
                    int[] fScores_current = GetScores(currentTile, start, finish);
                    if (GridManager.Instance.GetTileAtPosition(neighbour) != null && GridManager.Instance.GetTileAtPosition(neighbour).Walkable) {
                           int[]ghfScore_neighbour = GetScores(neighbour, start, finish);
                           if (closedList.Contains(neighbour)) {
                            continue; //Go to next adjacent square
                           } 
                           else if (!openList.Contains(neighbour)) {
                            //Compute its score, set the parent
                            openList.Add(neighbour);
                            parent = current;
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
        }

    private int[] GetScores(Vector2 coords, Vector2 start, Vector2 finish) {
        int g_score = Math.Sqrt((coords[0] - finsih[0])**2 + (coords[1] - finish[1])**2);
        int h_score = Math.Sqrt((coords[0] - start[0])**2 + (coords[1] - start[1])**2);
        int[] ghfScores = {g_score, h_score, g_score + h_score};
        return ghfScores;
    }

}