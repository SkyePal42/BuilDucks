using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                break;
            case GameState.ColleagueTurn:
                ColleagueManager.DoMischief();
                break;
            case GameState.AnimalsTurn:
                break;
            case GameState.EvaluationPhase:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
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
