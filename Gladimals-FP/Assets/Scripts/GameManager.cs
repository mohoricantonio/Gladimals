using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject aboutUI;
    public GameObject menuUI;
    public GameObject mainMenu;


    public void Play()
    {
        mainMenu.SetActive(false);
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
