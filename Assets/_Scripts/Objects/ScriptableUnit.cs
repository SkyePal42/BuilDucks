using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit",menuName = "Scriptable Unit")]
public class ScriptObject : ScriptableObject {
    public Faction Faction;
    public BaseUnit UnitPrefab;
}

public enum Type {
    Hero = 0,
    Enemy = 1
}