using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _grassTile, _waterTile, _gateHorTile, _gateVerTile, _fenceTile;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    private int _numberOfEntrances;
    private Vector2[] _entrances;

    void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();

        GenerateEntrances();
        GenerateFence();

        int waterCount = 3;
        bool water = false;

        for (int x = 1; x < _width - 1; x++)
        {
            for (int y = 1; y < _height - 1; y++)
            {
                // water tile chance calculation
                var randomTile = _grassTile;

                if (waterCount > 0 && Random.value > 0.9)
                {
                    randomTile = _waterTile;
                    water = true;
                    waterCount -= 1;
                }

                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";


                spawnedTile.Init(x, y);


                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

<<<<<<< HEAD
    public Tile GetHeroSpawnTile() {
        // Possibly more efficient get random https://stackoverflow.com/questions/40412340/c-sharp-dictionary-get-item-by-index
        return _tiles.Where(t => t.Value.Walkable).OrderBy(t => Random.value).First().Value;
=======
    public Tile GetHeroSpawnTile()
    {
        return _tiles.Where(t => t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
>>>>>>> eca2474cb29bb7abe0615974cca8658994a3246b
    }

    public Tile GetEnemySpawnTile()
    {
        return _tiles.Where(t => t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }
    

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

<<<<<<< HEAD
    public Tile GetRandomPosition() {
        var walkableTiles = _tiles.Where(t => t.Value.Walkable);
        var selectedIndex = Random.Range(0, walkableTiles.Count());
        return walkableTiles.ElementAt(selectedIndex).Value;
=======
    private void GenerateEntrances()
    {
        float random = Random.value;
        if (random < 0.25f)
        {
            _entrances = new Vector2[3];
            _entrances[2] = new Vector2((Random.value > 0.5f ? 0 : _width - 1), Random.Range(1, _height - 2));
        }
        else if (random > 0.90f)
        {
            _entrances = new Vector2[4];
            _entrances[2] = new Vector2(0, Random.Range(1, _height - 2));
            _entrances[3] = new Vector2(_width - 1, Random.Range(1, _height - 2));
        }
        else _entrances = new Vector2[2];
        _entrances[0] = new Vector2(Random.Range(1, _width - 2), _height - 1);
        _entrances[1] = new Vector2(Random.Range(1, _width - 2), 0);
    }

    private void GenerateFence()
    {
        for (int x = 0; x < _height; x++)
        {
            int[] ys = { 0, _height - 1 };
            foreach (var y in ys)
            {
                bool fence = true;
                Array.ForEach(_entrances, position => { if (new Vector2(x, y) == position) { fence = false; } });
                var tile = fence ? _fenceTile : _gateHorTile;
                var spawnedTile = Instantiate(tile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.Init(x, y);
                _tiles[new Vector2(x, y)] = spawnedTile;

            }
        }

        for (int y = 0; y < _height; y++)
        {
            int[] xs = { 0, _width - 1 };
            foreach (var x in xs)
            {
                bool fence = true;
                Array.ForEach(_entrances, position => { if (new Vector2(x, y) == position) { fence = false; } });
                var tile = fence ? _fenceTile : _gateVerTile;
                var spawnedTile = Instantiate(tile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.Init(x, y);
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
    }
}