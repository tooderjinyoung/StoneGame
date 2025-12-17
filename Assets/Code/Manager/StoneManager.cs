using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : SingletonDestroy<StoneManager>
{
    [SerializeField]
    private GameObject[] stoneGroups = new GameObject[2];
    
    private int[] ints= new int[2];

    public int[] currntCount()
    {
        ints[0] = stoneGroups[0].transform.childCount;
        ints[1] = stoneGroups[1].transform.childCount;

        return ints;
    }
    public bool IsAllStrop()
    {

        if (stoneGroups == null ) return true;

        foreach (var stone in stoneGroups)
        {
            Rigidbody2D rb = stone.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null && rb.velocity.sqrMagnitude > 0.05f)
            {
                rb.velocity = Vector2.zero;

                rb.angularVelocity = 0f;

                rb.rotation = 0f;
                return false;
            }
        }
        return true;
    }
}
