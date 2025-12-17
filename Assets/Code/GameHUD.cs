using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiName;
    [SerializeField] private TextMeshProUGUI uiTimer;
    [SerializeField] private TextMeshProUGUI uiRound;
    [SerializeField] private TextMeshProUGUI uiWin;

    void Update()
    {
        if (GameManager.Inst == null) return; 

        if (!GameManager.Inst.isPAUSE)
        {
            if (uiName != null) uiName.text = GameManager.Inst.currentState.ToString();
            if (uiTimer != null) uiTimer.text = $"Time: {GameManager.Inst.turnTimer:F2}";
            if (uiRound != null) uiRound.text = $"Round: {GameManager.Inst.leftover}";
        }

        if (GameManager.Inst.currentState == GameState.GAMEOVER)
        {
            if (uiWin != null) uiWin.text = GameManager.Inst.win;
        }
    }
}