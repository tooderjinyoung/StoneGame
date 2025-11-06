using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : Singleton<GameManager>
{
    private string currentPattern;
    public void OnButtonClick(string pattern)
    {
        currentPattern = pattern;
    }
    public string getPattern()
    {
        return this.currentPattern;
    }
}
