using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject aboutUI;
    public GameObject menuUI;
    public GameObject mainMenu;


    public void Play()
    {
        mainMenu.SetActive(false);
        SceneManager.LoadScene("enemy scene");
    }

    public void AboutMenu()
    {
        menuUI.SetActive(false);
        aboutUI.SetActive(true);
    }

    public void MainMenu()
    {
        aboutUI.SetActive(false);
        menuUI.SetActive(true);
    }
}
