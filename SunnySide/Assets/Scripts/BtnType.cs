using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BtnType : MonoBehaviour
{

    [SerializeField] private AudioSource button_press;
    public GameObject How_To_pop;
    public GameObject btn_menu;
    public BTNType currentType;

    public void OnBtnClick()
    {
        switch (currentType)
        {

            case BTNType.Start:

                Debug.Log("시작");
                SceneManager.LoadScene("SampleScene");
                button_press.Play();

                break;

            case BTNType.How_To:

                How_To_pop.SetActive(true);
                btn_menu.SetActive(false);
                button_press.Play();


                break;

            case BTNType.Exit:
                
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit()
            #endif

                break;

            case BTNType.Quit:
               

                  How_To_pop.SetActive(false);
                  btn_menu.SetActive(true);
               
                button_press.Play();

                break;
        }
    }
}

