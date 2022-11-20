using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class ColleagueManager : MonoBehaviour
{
    private static Tile GetRandomPosition()
    {
        var walkableTiles = GridManager.Instance.GetTiles().Where(t => t.Value.Walkable == true && t.Value.OccupiedObject == null && t.Value.OccupiedAnimal == null);
        var selectedIndex = Random.Range(0, walkableTiles.Count());
        return walkableTiles.ElementAt(selectedIndex).Value;
    }
    private static GameObject GetRandomObject()
    {
        var objects = GridManager.Instance._objects;//.Where(o => GameManager.Instance.SpeculateExpense(o.GetComponent<BaseObject>().cost) < Mathf.FloorToInt(GameManager.Instance.GetMoney() / 2));
        var selectedIndex = Random.Range(0, objects.Count());
        return objects.ElementAt(selectedIndex);
    }
    public static void DoMischief()
    {
        var tile = GetRandomPosition();
        var instance = Instantiate(GetRandomObject(), tile.transform);
        tile.OccupiedObject = instance.GetComponent<BaseObject>();
        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }
}