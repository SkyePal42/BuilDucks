using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : BaseObject
{
     public override int Judge()
    // return int val of how good the placement of the object is
    {
        return look;
    }
}