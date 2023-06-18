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
    public GameObject player;
    public GameObject enemy;

    private void Start() {
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
    }

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
    void CheckEndGame()
    {
        if (player.GetComponent<PlayerHealth>().isDead() && !endGame){
            endGame = true;
            player.GetComponentInChildren<Animator>().SetTrigger("Death");
            player.GetComponent<PlayerCombat>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
            enemy.GetComponent<EnemyMovement>().StopMoovingAnimations();
            enemy.GetComponent<EnemyMovement>().StopAttackingAnimations();
            enemy.GetComponent<EnemyMovement>().enabled = false;
            enemy.GetComponent<EnemyAttack>().enabled = false;
        }
    }

    void FixedUpdate()
    {
        CheckEndGame();

        if(endGame) 
        {
            Cursor.visible = true;
            //Time.timeScale = 0f;
        }
    }
    // ------END------
}
