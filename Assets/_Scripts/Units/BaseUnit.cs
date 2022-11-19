using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour {
    public string UnitName;
    public Tile OccupiedTile;
    public Faction Faction;

    public virtual void MoveRandom() {
        Tile destinationTile = GridManager.Instance.GetRandomPosition();
        Debug.Log(destinationTile);
        }   
    }

