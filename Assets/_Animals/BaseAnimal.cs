using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimal : MonoBehaviour {
    public string UnitName;
    public Tile OccupiedTile;

    public virtual void MoveRandom() {
        Tile destinationTile = GridManager.Instance.GetRandomPosition();
        Debug.Log(destinationTile);
    }

}