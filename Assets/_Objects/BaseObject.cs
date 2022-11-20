using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  BaseObject : MonoBehaviour
{
    public string ObjectName;
    private Vector2 position;
    public int cost; //might not need to be public
    public int look; // 0-10
    public int lightStrength; //0-10
    public int pollutionStrength; // 0-10
    public bool walk;
    public bool sit;
    public bool swim;
    public enum ObjectTypes
    {
        NULL = 0,
        BENCH = 1,
        BIN = 2,
        BUSH = 3,
        FLOWER = 4,
        FOUNTAIN = 5,
        LAMP = 6,
        PATH = 7,
        TOILET = 8,
        TREE = 9
    }
    public ObjectTypes ObjectType = ObjectTypes.NULL;
    public static Dictionary<ObjectTypes,List<BaseObject>> ObjectsList = new Dictionary<ObjectTypes,List<BaseObject>>();
    // Puts the object down
    // drag and drop or clicks?

    protected BaseObject() {
        ObjectsList[ObjectType].Add(this);
    }
    public virtual bool CanPlace(Tile ground)
// Puts the object down
    {  
    if (ground.Walkable == true && ground.OccupiedObject == null && ground.OccupiedAnimal == null)
    {
        return true;
    }
    return false;
    }

    public virtual int Judge()
    // return int val of how good the placement of the object is
    {
        return 0;
    }
}