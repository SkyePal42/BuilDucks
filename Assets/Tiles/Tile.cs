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

    public BaseUnit OccupiedUnit;
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

        if (OccupiedUnit != null) {
            if(OccupiedUnit.Faction == Faction.Hero) UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
            else {
                if (UnitManager.Instance.SelectedHero != null) {
                    var enemy = (BaseEnemy) OccupiedUnit;
                    Destroy(enemy.gameObject);
                    UnitManager.Instance.SetSelectedHero(null);
                }
            }
        }
        else {
            if (UnitManager.Instance.SelectedHero != null) {
                SetUnit(UnitManager.Instance.SelectedHero);
                UnitManager.Instance.SetSelectedHero(null);
            }
        }

    }

    public void SetUnit(BaseUnit unit) {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
}