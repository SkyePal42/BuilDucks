using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent class to all objects
//Public or private functions?
public class NewBehaviourScript : MonoBehaviour
{
    private Vector2 position;
    public int cost; //might not need to be public
    public bool walk;
    public bool sit; 
    public bool swim;
    private int look; // 0-10
    private int lightStrength; //0-10
    private int pollutionStrength; // 0-10

    public void Init(int x, int y)
    // Sets values to most common defaults, some will be overriden
    {
        position = new Vector2(x,y);
        cost = 0; // basic is free
        walk = true;
        sit = true; // should ground be sit-able?
        swim = false;
        look = 0; // 0-10
        lightStrength = 0; //0-10
        pollutionStrength = 0; // 0-10
        //int [] prices = [0,10,20]; //check how variations are being implemented.
    }

    public bool IsWalkable()
    {
        return walk;
    }

    public bool IsSwimmable()
    {
        return swim;
    }

    public bool IsSittable()
    {
        return sit;
    }

    public int Cost()
    {
        return cost;
    }

    public

    void Place(ground)
    // Puts the object down
    {
        ;
    }

    int Judge()
    // return int val of how good the placement of the object is
    {
        return 0;
    }
}
