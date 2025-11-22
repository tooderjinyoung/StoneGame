using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] backGroundObject = new GameObject[5];
    string Background;
    private void Start()
    {


        if (GameManager.Inst != null)
        {
            Background = GameManager.Inst.selectBackground;
        }

            for (int i = 0; i < backGroundObject.Length; i++)
            {
                if (backGroundObject[i].name == Background)
                {
                    Instantiate(backGroundObject[i]);
                }
            }
    }
}
