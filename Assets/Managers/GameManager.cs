using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;
    public GameObject selectedObject;
    [SerializeField] private int _Money = 100000000;
    private List<BaseTask> _Tasks;
    public List<BaseTask> _CurrentTasks = new List<BaseTask>();

    void Awake()
    {
        Instance = this;
        _Tasks = Resources.LoadAll<BaseTask>("Tasks").ToList();
    }

    void Start()
    {
        ChangeState(GameState.TaskAssignment);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        MenuManager.Instance.ChangeState(GameState.ToString());
        switch (newState)
        {
            case GameState.TaskAssignment:
                AssignTasks(2);
                break;
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnAnimals:
                AnimalManager.Instance.SpawnAnimals();
                break;
            case GameState.PlayerTurn:
                MenuManager.Instance._endGame.interactable = true;
                MenuManager.Instance.EnableAll();
                break;
            case GameState.ColleagueTurn:
                StartCoroutine("Mischief");
                break;
            case GameState.AnimalsTurn:
                AnimalManager.Instance.StartCoroutine("Move");
                break;
            case GameState.EvaluationPhase:
                Evaluation();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    // ! Task Assignment Code

    private void AssignTasks(int numOfTasks)
    {
        for (int i = 0; i < numOfTasks; i++)
        {
            var candidates = _Tasks.Where(t => !Array.Exists(_CurrentTasks.ToArray(), n => n == t) && Random.value < t.Probability).ToList();
            if (candidates.Count > 0) _CurrentTasks.Add(candidates.First());
        }
        MenuManager.Instance.UpdateTasks();
        ChangeState(GameState.GenerateGrid);
    }

    // ! Money Stuff

    public int GetMoney()
    {
        return _Money;
    }
    public bool RemoveMoney(int expense)
    {
        if (_Money - expense >= 0)
        {
            _Money -= expense;
            MenuManager.Instance.UpdateMoney();
            return true;
        }
        return false;
    }
    public int SpeculateExpense(int expense)
    {
        return _Money - expense;
    }

    // ! Colleague Code
    private Tile GetRandomPosition()
    {
        var walkableTiles = GridManager.Instance.GetTiles().Where(t => t.Value.Walkable == true && t.Value.OccupiedObject == null && t.Value.OccupiedAnimal == null);
        var selectedIndex = Random.Range(0, walkableTiles.Count());
        return walkableTiles.ElementAt(selectedIndex).Value;
    }
    private GameObject GetRandomObject()
    {
        var objects = GridManager.Instance._objects;//.Where(o => GameManager.Instance.SpeculateExpense(o.GetComponent<BaseObject>().cost) < Mathf.FloorToInt(GameManager.Instance.GetMoney() / 2));
        var selectedIndex = Random.Range(0, objects.Count());
        return objects.ElementAt(selectedIndex);
    }
    IEnumerator Mischief()
    {
        yield return new WaitForSeconds(0.5f);
        var tile = GetRandomPosition();
        var instance = Instantiate(GetRandomObject(), tile.transform);
        tile.OccupiedObject = instance.GetComponent<BaseObject>();
        GameManager.Instance.RemoveMoney(instance.GetComponent<BaseObject>().cost);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.ChangeState(GameState.AnimalsTurn);
    }

    // ! Judgement Code

    private void Evaluation()
    {
        int humanTotal = 0;
        for (int i = 0; i < BaseObject.ObjectsList.Count(); i++)
        {
            BaseObject.ObjectsList.ElementAt(i).Value.ForEach(o => humanTotal += o.Judge());
        }

        _CurrentTasks.ForEach(t => {
            if (!BaseObject.ObjectsList.ContainsKey(t.typeOfObject) || BaseObject.ObjectsList[t.typeOfObject].Count < t.numberOfObjects) humanTotal += t.TaskPenalty;
            else humanTotal += t.TaskReward;
        });

        int natureTotal = 0;
        BaseAnimal sacrifice = AnimalManager.Instance._animalInstances[0];
        for (int i = 0; i < GridManager.Instance._lakes.Count; i++)
        {
            for (int n = i + 1; n < GridManager.Instance._lakes.Count; n++)
            {
                if (i != n)
                {
                    GridManager.Instance.GetTileAtPosition(GridManager.Instance._lakes[i]).SetAnimal(sacrifice);
                    var result = sacrifice.FindPath(GridManager.Instance._lakes[n]);
                    natureTotal += (result == null ? -10 : 1);
                    // if (result != null) Debug.Log(result[result.Count - 1].PrintString()); else Debug.Log("Returned Null!");
                }
                else
                {
                    Debug.Log("Same Tile Lol");
                }

            }
        }
        MenuManager.Instance.EndGame(humanTotal, natureTotal);
    }

    public void EndGame() { MenuManager.Instance._endGame.interactable = false; ChangeState(GameState.EvaluationPhase); }
}

public enum GameState
{
    TaskAssignment = -1,
    GenerateGrid = 0,
    SpawnAnimals = 1,
    PlayerTurn = 2,
    ColleagueTurn = 3,
    AnimalsTurn = 4,
    EvaluationPhase = 5
}
