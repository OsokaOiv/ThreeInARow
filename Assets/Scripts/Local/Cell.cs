using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int[] index = new int[2];

    private bool clicked = false;
    private GameObject[] sprites = new GameObject[2];

    public void Awake()
    {
        sprites[0] = this.gameObject.transform.GetChild(0).gameObject;
        sprites[1] = this.gameObject.transform.GetChild(1).gameObject;
    }
    public void ActivatePlayerSprite(int player)
    {
        if (!clicked)
        {
            clicked = true;
            sprites[player].SetActive(true);
        }
    }

    public void ResetSprite()
    {
        clicked = false;
        sprites[0].SetActive(false);
        sprites[1].SetActive(false);
    }
}
