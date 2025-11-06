using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectArrangemet : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    public void clickBtn()
    {
        GameObject clickObject =EventSystem.current.currentSelectedGameObject;

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager component not found on the GameObject.");
            return;
        }
        gameManager.OnButtonClick(clickObject.name);
        Debug.Log(clickObject.name);
    }
}
