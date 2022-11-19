using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private GameObject _selectedHeroObject, _tileObject, _tileUnitObject;
    [SerializeField] private TextMeshProUGUI _money;
    [SerializeField] private GameObject[] _Sets;
    private GameObject currentSet = null;
    [SerializeField] private Button closeObjects;
    void Awake()
    {
        Instance = this;
    }

    public void ShowSet(int setIndex)
    {
        if (currentSet != null) { currentSet.SetActive(false);}
        _Sets[setIndex].SetActive(true);
        currentSet = _Sets[setIndex];
        if (!closeObjects.interactable) closeObjects.interactable = true;
    }

    public void CloseSet()
    {
        currentSet.SetActive(false);
        closeObjects.interactable = false;
    }

    public void ShowTileInfo(Tile tile)
    {

        if (tile == null)
        {
            _tileObject.SetActive(false);
            _tileUnitObject.SetActive(false);
            return;
        }

        _tileObject.GetComponentInChildren<Text>().text = tile.TileName;
        _tileObject.SetActive(true);

        if (tile.OccupiedObject)
        {
            _tileUnitObject.GetComponentInChildren<Text>().text = tile.OccupiedObject.ObjectName;
            _tileUnitObject.SetActive(true);
        }
    }

    public void ShowSelectedHero(BaseAnimal hero)
    {
        if (hero == null)
        {
            _selectedHeroObject.SetActive(false);
            return;
        }

        _selectedHeroObject.GetComponentInChildren<Text>().text = hero.UnitName;
        _selectedHeroObject.SetActive(true);
    }
}
