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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        MenuManager.Instance.ChangeState(GameState.ToString());
        switch (newState)
        {
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
        int total = 0;
        for (int i = 0; i < BaseObject.ObjectsList.Count(); i++)
        {
            BaseObject.ObjectsList.ElementAt(i).Value.ForEach(o => total += o.Judge());
        }
        MenuManager.Instance.EndGame(total,0);
    }

    public void EndGame() { MenuManager.Instance._endGame.interactable = false; ChangeState(GameState.EvaluationPhase); }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnAnimals = 1,
    PlayerTurn = 2,
    ColleagueTurn = 3,
    AnimalsTurn = 4,
    EvaluationPhase = 5
}
