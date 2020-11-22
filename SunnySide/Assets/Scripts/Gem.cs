using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gem : MonoBehaviour
{
    public GameObject clearMenu;

    private void Start()
    {
        clearMenu.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            clearMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void nextStage()
    {
        string scenename = SceneManager.GetActiveScene().name;
        // GameObject.Find("Main Camera").GetComponent<BackgroundMusic>().musicChange();

        switch (scenename)
        {
            case "SampleScene":
                SceneManager.LoadScene("scenes1");
                Time.timeScale = 1;
                break;
            case "scenes1":
                // SceneManager.LoadScene("scenes1");
                Time.timeScale = 1;
                break;
        }
    }

}
