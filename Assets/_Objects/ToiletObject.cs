using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletObject : BaseObject
{
     public override int Judge()
    // return int val of how good the placement of the object is
    {
        int score = look;
        int PathTolerance = 2;
        int closestPath = 0;
        BaseObject.ObjectsList[ObjectTypes.PATH].ForEach(o => {if ((Mathf.Abs(transform.position.x - o.transform.position.x)+Mathf.Abs(transform.position.x - o.transform.position.x)) < closestPath)
        {
            closestPath = Mathf.RoundToInt(Mathf.Abs(transform.position.x - o.transform.position.x)+Mathf.Abs(transform.position.x - o.transform.position.x));
        }});
        if (closestPath > PathTolerance) score -= closestPath - PathTolerance;
        return score;
    }
}
