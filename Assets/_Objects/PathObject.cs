using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObject : BaseObject
{
    public override int Judge()
    // return int val of how good the placement of the object is
    {
        int score = look;
        BaseObject.ObjectsList[ObjectTypes.FLOWER].ForEach(o => {if (Vector2.Distance(transform.position,o.transform.position) <= 1) score += 1;});
        BaseObject.ObjectsList[ObjectTypes.LAMP].ForEach(o => {if (Vector2.Distance(transform.position,o.transform.position) <= o.lightStrength) score += 1;});
        return score;
    }
}