using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // ------BEGIN------
    // Initial Menu scene variables and functions

    [Header("Menu Variables")]
    public GameObject aboutUI;
    public GameObject menuUI;
    public GameObject mainMenu;


    public void Play()
    {
        mainMenu.SetActive(false);
        SceneManager.LoadScene("HealthBar");
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
    // ------END------

    // ------BEGIN------
    // Game Scene variables and functions

    [Header("Game Variables")]
    
    bool endGame = false;

    bool CheckEndGame()
    {
        PlayerHealth playerHP = GameObject.Find("Player").GetComponent<PlayerHealth>();
        if(playerHP.currentHealth <= 0) return true;
        else return false;
    }

    void FixedUpdate()
    {
        endGame = CheckEndGame();

        if(endGame) Time.timeScale = 0f;
    }
    // ------END------
}
