using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class SelectBackGround : MonoBehaviour
{

    [SerializeField]
    Sprite[] backGroundObject = new Sprite[5];
    private Image backGround;
    TextMeshProUGUI text;
    public float timer = 5f;
    private float speed = 0.1f;


    private void Start()
    {
        GameObject StoneArrangement = GameObject.Find("StoneArrangement");

        text = GameObject.Find("Name").GetComponent<TextMeshProUGUI>();
        if (GameManager.Inst == null)
        {
            Debug.LogError("GameManager가 아직 초기화되지 않았습니다!");
            return;
        }

        backGround = GetComponent<Image>();
        StartCoroutine(enumerator());
    }

    IEnumerator enumerator()
    {
        while (timer > 1f)
        {
            int randomIndex = Random.Range(0, backGroundObject.Length);
            backGround.sprite = backGroundObject[randomIndex];
            yield return new WaitForSeconds(speed);
            speed += 0.01f;
            timer *= 0.96f;
            text.text = backGround.sprite.name;
        }
        GameManager.Inst.OnButtonClick(text.text);
        Debug.Log("Selected Background: " + text.text);
        yield return new WaitForSeconds(3f);
        GameObject BackGroundArrangement = GameObject.Find("BackGroundArrangement");
        BackGroundArrangement.transform.localScale = new Vector3(0, 0, 0);
        GameObject StoneArrangement = GameObject.Find("StoneArrangement");
        StoneArrangement.transform.localScale = new Vector3(1,1,1);
    }
}
