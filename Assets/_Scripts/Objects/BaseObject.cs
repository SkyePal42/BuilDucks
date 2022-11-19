using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  BaseObject : MonoBehaviour
{
    private Vector2 position;
    public int cost; //might not need to be public
    public int look; // 0-10
    public int lightStrength; //0-10
    public int pollutionStrength; // 0-10
    public bool walk;
    public bool sit;
    public bool swim;
    public virtual void Place(Tile ground)
    // Puts the object down
    {
        
    }

    public virtual int Judge()
    // return int val of how good the placement of the object is
    {
        return 0;
    }
}
