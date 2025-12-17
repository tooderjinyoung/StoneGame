using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement   ;
    
public class ChangeScenes : MonoBehaviour
{
    private Dictionary<string, string> titleMap = new Dictionary<string, string>()
    {
        {"Start","SettingSenes" },
        { "BackGround", "White" },
        { "White", "Black" },
        { "Black", "LoadSences" }
    };

    private void Change(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void NextScene()
    {
        if (GameManager.Inst == null)
        {
            Debug.LogError("GameManager가 아직 초기화되지 않았습니다!");
            return;
        }

        TextMeshProUGUI text = GameObject.Find("Arrangement").GetComponent<TextMeshProUGUI>();
        Dictionary<string, string> pattern = GameManager.Inst.arrangent;
        string currentTitle = text.text;

        if (titleMap.ContainsKey(currentTitle))
        {
            // 다음 타이틀로 이동
            string nextTitle = titleMap[currentTitle];

            if (pattern[currentTitle] == null)
            {
                Debug.LogError("No pattern found");
                return;
            }
            if (nextTitle.Contains("Sences"))
            {
                Change(nextTitle);
                return;
            }
            text.text = nextTitle;
        }
        else
        {
            Change(titleMap[currentTitle]);
            return;
        }
    }
}
