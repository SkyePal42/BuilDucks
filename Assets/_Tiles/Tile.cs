using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public enum TileTypes
    {
        Grass = 0,
        Water = 1,
        Fence = 2,
        Gate = 3
    }
    public TileTypes _tileType;
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    public BaseAnimal OccupiedAnimal;
    public BaseObject OccupiedObject;
    public bool Walkable => _tileType == TileTypes.Grass && (OccupiedObject == null || OccupiedObject.walk);
    public bool Swimmable => _tileType == TileTypes.Water && (OccupiedObject == null || OccupiedObject.swim);

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    void OnMouseDown() {
        if(GameManager.Instance.GameState != GameState.HeroesTurn) return;

        if (OccupiedObject == null && GameManager.Instance.selectedObject != null && GameManager.Instance.selectedObject.GetComponent<BaseObject>().CanPlace(this) && GameManager.Instance.RemoveMoney(GameManager.Instance.selectedObject.GetComponent<BaseObject>().cost)) {
            var instance = Instantiate(GameManager.Instance.selectedObject,transform);
            OccupiedObject = instance.GetComponent<BaseObject>();
        }
    }

    public void SetAnimal(BaseAnimal animal) {
        if (animal.OccupiedTile != null) animal.OccupiedTile.OccupiedAnimal = null;
        animal.transform.position = transform.position;
        OccupiedAnimal = animal;
        animal.OccupiedTile = this;
    }
}