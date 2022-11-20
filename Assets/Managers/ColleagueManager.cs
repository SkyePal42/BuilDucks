using UnityEngine;

public class ColleagueManager : MonoBehaviour {
    public static void DoMischief() {
        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }
}