using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearMenu : MonoBehaviour
{
    public Sprite[] Numbers = new Sprite[10];
    public int digit;
    public Image image;
    PlayerController player;
    public int wholeCherry;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    
    private void Update()
    {
        if (digit == 10)
        {
            image.sprite = Numbers[player.cherries / 10];
        }
        else if (digit == 1)
        {
            image.sprite = Numbers[player.cherries % 10];
        }
        else if(digit == 2)
        {
            image.sprite = Numbers[wholeCherry / 10];
        }
        else if(digit == 3)
        {
            image.sprite = Numbers[wholeCherry % 10];
        }

    }


}
