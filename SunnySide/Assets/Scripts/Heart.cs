using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public Sprite[] hearts = new Sprite[2];
    public int number;
    public Image image;
    PlayerController player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(player.health >= number)
        {
            image.sprite = hearts[0];
        }
        else
        {
            image.sprite = hearts[1];
        }
    }
}
