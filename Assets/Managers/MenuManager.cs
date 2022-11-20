using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private GameObject _selectedHeroObject, _tileObject, _tileUnitObject;
    [SerializeField] private TextMeshProUGUI _money, _turn, _selected, _huamnScore, _natureScore;
    [SerializeField] private GameObject[] _Sets, _Options, _EndGame;
    private GameObject currentSet = null;
    [SerializeField] private Button closeObjects;
    public Button _endGame;
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public void UpdateTasks()
    {
        foreach (BaseTask task in GameManager.Instance._CurrentTasks)
        {
            GameObject newTask = Instantiate(ListItemPrefab, ContentPanel.transform) as GameObject;
            ListItemController controller = newTask.GetComponent<ListItemController>();
            controller.Name.text = task.TaskName;
            controller.Description.text = task.TaskDescription;
            controller.Reward.text = task.TaskReward.ToString();
            controller.Penalty.text = task.TaskPenalty.ToString();
            newTask.transform.localScale = Vector3.one;
            newTask.GetComponent<RectTransform>().sizeDelta = new Vector2(200,150);
        }
    }
    public void EndGame(int human, int nature)
    {
        foreach (var item in _EndGame)
        {
            item.SetActive(true);
        }
        _huamnScore.text = human.ToString();
        _natureScore.text = nature.ToString();
    }
    void Awake()
    {
        Instance = this;
        UpdateMoney();
        SelectObject(null);
    }
    public void DisableAll()
    {
        for (int i = 0; i < _Options.Length; i++)
        {
            _Options[i].GetComponent<Button>().interactable = false;
        }
    }
    public void EnableAll()
    {
        for (int i = 0; i < _Options.Length; i++)
        {
            _Options[i].GetComponent<Button>().interactable = true;
        }
    }

    public void ShowSet(int setIndex)
    {
        if (currentSet != null) { currentSet.SetActive(false); }
        _Sets[setIndex].SetActive(true);
        currentSet = _Sets[setIndex];
        if (!closeObjects.interactable) closeObjects.interactable = true;
    }

    public void CloseSet()
    {
        currentSet.SetActive(false);
        closeObjects.interactable = false;
        SelectObject(null);
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

    // for clicking button, pass in null when closing instead of opening. CloseSet() already exists, is this necessary?
    public void SelectObject(GameObject obj)
    {
        GameManager.Instance.selectedObject = obj;
        _selected.text = obj == null ? "" : obj.GetComponent<BaseObject>().ObjectName;
    }

    public void UpdateMoney()
    {
        _money.text = GameManager.Instance.GetMoney().ToString();
    }

    public void ChangeState(string newState)
    {
        _turn.text = newState;
    }

}