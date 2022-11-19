using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour {
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable;
    [SerializeField] private bool _isSwimmable;

    public BaseUnit OccupiedUnit;
    public BaseObject OccupiedObject;
    public bool Walkable => _isWalkable && (OccupiedObject == null || OccupiedObject.walk);
    public bool Swimmable => _isSwimmable && (OccupiedObject == null || OccupiedObject.swim);


    public virtual void Init(int x, int y)
    {
      
    }

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