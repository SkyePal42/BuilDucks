using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushObject : BaseObject
{
     public override int Judge()
    // return int val of how good the placement of the object is
    {
        return look;
    }
}