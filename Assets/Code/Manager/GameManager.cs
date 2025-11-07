using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : Singleton<GameManager>
{

    private Dictionary<string, GameObject> stonePrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, string> arrangent = new Dictionary<string, string>();

    private void DoAwake() 
    {
        stonePrefabs["White"] = Resources.Load<GameObject>("Assets/Prefeb/R_White.prefab");
        stonePrefabs["Black"] = Resources.Load<GameObject>("Assets/Prefeb/R_Black.prefab");
    }

    public void OnButtonClick(string pattern,string color)
    {
        arrangent[color] = pattern;
    }
    public Dictionary<string,string> getPattern()
    {
        return this.arrangent;
    }
}
