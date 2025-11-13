using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : Singleton<GameManager>
{

    private Dictionary<string, string> arrangent = new Dictionary<string, string>();
    private string  selectBackground =null;

    public void OnButtonClick(string pattern,string color)
    {
        arrangent[color] = pattern;
    }
    public void OnButtonClick(string background)
    {
        this.selectBackground = background;
    }
    public Dictionary<string,string> getPattern()
    {
        return this.arrangent;
    }
    public string getBackground()
    {
        return this.selectBackground;
    }
}
