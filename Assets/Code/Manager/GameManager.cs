using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : Singleton<GameManager>
{

    public Dictionary<string, string> arrangent { get; private set; } = new Dictionary<string, string>();
    public string  selectBackground { get; private set; } = "Spring";
    public string my_color { get; private set; } = "Black";

    public void OnButtonClick(string pattern,string color)
    {
        arrangent[color] = pattern;
    }
    public void OnButtonClick(string background)
    {
        this.selectBackground = background;
    }
    public void OnButtonClick_color(string color)
    {
        this.my_color = color;
    }
}
