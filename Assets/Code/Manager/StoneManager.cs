using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : SingletonDestroy<StoneManager>
{
    [SerializeField]
    private Transform[] stoneGroups;


    public bool IsAllStrop()
    {
        if(stoneGroups == null ) return true;

        foreach (var stone in stoneGroups)
        {
            Rigidbody2D rb = stone.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null && rb.velocity.sqrMagnitude > 0.05f)
            {
                Debug.Log(stone + " µé¾î¿È");
                return false;
            }
        }
        return true;
    }
}
