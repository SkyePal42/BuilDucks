using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;

    private List<ScriptableAnimal> _units;
    public BaseAnimal SelectedHero;

    void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<ScriptableAnimal>("Animals").ToList();

    }

    public void SpawnAnimals()
    {
        _units.ForEach(u =>
        {
            for (int i = 0; i < u.numberOnMap; i++)
            {
                var spawnedAnimal = Instantiate(u.AnimalPrefab);
                var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();
                randomSpawnTile.SetAnimal(spawnedAnimal);
            }
        });
        GameManager.Instance.ChangeState(GameState.HeroesTurn);
    }
}
