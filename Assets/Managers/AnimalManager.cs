using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;

    private List<ScriptableAnimal> _animals;
    private List<BaseAnimal> _animalInstances = new List<BaseAnimal>();
    public BaseAnimal SelectedHero;

    void Awake()
    {
        Instance = this;

        _animals = Resources.LoadAll<ScriptableAnimal>("Animals").ToList();

    }

    public void SpawnAnimals()
    {

        _animals.ForEach(u =>
        {
            for (int i = 0; i < u.numberOnMap; i++)
            {
                var spawnedAnimal = Instantiate(u.AnimalPrefab);
                var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();
                randomSpawnTile.SetAnimal(spawnedAnimal);
                
                   _animalInstances.Add(spawnedAnimal.GetComponent<BaseAnimal>());
            }
        });
        
        AnimalManager.Instance.MoveAbout();
        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }

    public void MoveAbout()
    {
        /*
        foreach (BaseAnimal animal in _animalInstances) {
            Debug.Log(animal);
        }
        */

        BaseAnimal duck = _animalInstances[0];
        List<PathNode> shortestPath = duck.FindPath();
        Debug.Log(shortestPath[1].x.ToString() +","+ shortestPath[1].y.ToString());
    }

}
