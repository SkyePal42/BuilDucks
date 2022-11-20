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
        AnimalManager.Instance.StartCoroutine("Move");
    }

    private IEnumerator Move()
    {
        for (int i = 0; i < _animalInstances.Count; i++)
        {
            List<PathNode> shortestPath;
            do
            {
                shortestPath = _animalInstances[i].FindPath();
            } while (shortestPath != null && shortestPath.Count < 2);
            if (shortestPath != null)
            {
                float lerpDuration = 1f;
                float elapsed = 0;
                Vector3 start =
                _animalInstances[i].transform.position;
                Vector3 end = new Vector3(shortestPath[1].x, shortestPath[1].y, 0);
                while (elapsed < lerpDuration)
                {
                    elapsed += Time.deltaTime;
                    _animalInstances[i].transform.position = Vector3.Lerp(start, end, elapsed / lerpDuration);
                    yield return null;
                }
                GridManager.Instance.GetTileAtPosition(new Vector2(shortestPath[1].x, shortestPath[1].y)).SetAnimal(_animalInstances[i]);
            }
        }
        yield return new WaitForSeconds(1);
        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }

}
