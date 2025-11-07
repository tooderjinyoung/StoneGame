using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScenes : MonoBehaviour
{
    
    private void Change()
    {
        SceneManager.LoadScene("PlaySenes");
    }

    public void NextScene()
    {
        if (GameManager.Inst == null)
        {
            Debug.LogError("GameManager가 아직 초기화되지 않았습니다!");
            return;
        }

        Dictionary<string, string> pattern = GameManager.Inst.getPattern();
        if (pattern.Count == 2)
        {
            Change();
        }
        else
        {
            TextMeshProUGUI text = GameObject.Find("Arrangement").GetComponent<TextMeshProUGUI>();
            text.text = "Black";
        }
    }
}
