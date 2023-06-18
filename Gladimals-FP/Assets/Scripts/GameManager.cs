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

    // Loads the game scene
    public void PlayMenu()
    {
        mainMenu.SetActive(false);
        SceneManager.LoadScene("HealthBar");
    }

    // UI Objects Activation / Deactivation
    public void AboutMenu()
    {
        menuUI.SetActive(false);
        aboutUI.SetActive(true);
    }

    // UI Objects Activation / Deactivation
    public void MainMenu()
    {
        aboutUI.SetActive(false);
        menuUI.SetActive(true);
    }
    // ------END------

    // ------BEGIN------
    // Game Scene variables and functions

    [Header("Game Variables")]
    public GameObject gameOverUI;
    public GameObject healthBarUI;

    // Private Vriables
    bool endGame = false;

    // Reloads the scene to play again
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Gets back to main menu
    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Checks player health status and ends or not the game
    bool CheckEndGame()
    {
        PlayerHealth playerHP = GameObject.Find("Player").GetComponent<PlayerHealth>();
        if(playerHP.currentHealth <= 0)
        {
            healthBarUI.SetActive(false);
            gameOverUI.SetActive(true);
            return true;
        } 
        else return false;
    }

    void FixedUpdate()
    {
        endGame = CheckEndGame();

        if(endGame) 
        {
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }
    // ------END------
}
