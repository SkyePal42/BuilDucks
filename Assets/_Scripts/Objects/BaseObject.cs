using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {
    public string ObjectName;
    public Tile OccupiedTile;
    public int Cost;
    public bool Walkable;
    public bool Swimable;
    public bool Sittable;
    public int Look;
    public int LightStrength;
    public int PollutionStrength;
    public virtual void Place(){
        // Check if can place then place
        // Else let user know it is invalid
    }
    public virtual int Judge(){
        return 0;
    }
}
