using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task",menuName = "Scriptable Task")]
public class BaseTask : ScriptableObject {
    public string TaskName;
    public string TaskDescription;
    public int TaskReward;
    public int TaskPenalty;
    [Range(0f, 1f)]
    public float Probability;
    public BaseObject.ObjectTypes typeOfObject;
    public int numberOfObjects;
}