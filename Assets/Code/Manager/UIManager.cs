using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 이 스크립트를 Menu 프리팹에 붙이세요.
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel; // UIRestart 대신 명확한 이름 사용
    [SerializeField] private Slider soundSlider;

    public void OpenMenu() // Stop() 대신 명확한 이름
    {
        menuPanel.SetActive(true); // Scale 조절보다 SetActive가 더 일반적입니다.
        GameManager.Inst.isPAUSE = true;
    }

    public void CloseMenu() // Out() 대신 명확한 이름
    {
        menuPanel.SetActive(false);
        GameManager.Inst.isPAUSE = false;
    }

    public void OnVolumeChanged() // Save() 대신
    {
        if (soundSlider != null) AudioListener.volume = soundSlider.value;
    }
}