using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoneColor : MonoBehaviour
{
    public  void OnButtonClick_color()
    {

        string clickedName = EventSystem.current.currentSelectedGameObject.name;
        GameManager.Inst.OnButtonClick_color(clickedName);

        Debug.Log(clickedName);
        GameObject StoneArrangement = GameObject.Find("StoneSelect");

        StoneArrangement.transform.localScale = new Vector3(0, 0, 0);
        GameObject BackGroundArrangement = GameObject.Find("BackGroundArrangement");
        BackGroundArrangement.transform.localScale = new Vector3(1, 1, 1);

        Transform child = BackGroundArrangement.GetComponentsInChildren<Transform>(true)
                                .FirstOrDefault(t => t.name == "RandomSprite");
        child.gameObject.SetActive(true);
    }


}
