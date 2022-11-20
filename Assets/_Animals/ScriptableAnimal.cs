using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ducks walking on tiles that contain objects
[CreateAssetMenu(fileName = "New Unit",menuName = "Scriptable Animal")]
public class ScriptableAnimal : ScriptableObject {
    public BaseAnimal AnimalPrefab;
    public int numberOnMap;
}