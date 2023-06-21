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
    public GameObject howToPlayUI;

    // Loads the game scene
    public void PlayMenu()
    {
        mainMenu.SetActive(false);
        SceneManager.LoadScene(0);
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
        howToPlayUI.SetActive(false);
        aboutUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void HowToPlay()
    {
        menuUI.SetActive(false);
        howToPlayUI.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
    // ------END------

    // --------------------

    // ------BEGIN------
    // Game Scene variables and functions
    [Header("Game Variables")]
    public GameObject player;
    public GameObject enemy;
    public GameObject arena;
    public GameObject gameOverUI;
    public GameObject healthBarUI;
    public GameObject WinnerUI;
    public AudioClip crowdHeavySound;
    public GameObject pauseMenuUI;

    // Private Vriables
    private bool endGame = false;

    private void Start() {
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        arena = GameObject.Find("ArenaPrefab");
    }

    // Reloads the scene to play again
    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Gets back to main menu
    public void GoMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    // Checks player health status and ends or not the game
    void CheckEndGame()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;
        if (player.GetComponent<PlayerHealth>().isDead() && !endGame){
            endGame = true;
            player.GetComponentInChildren<Animator>().SetTrigger("Death");
            arena.GetComponentInChildren<AudioSource>().PlayOneShot(crowdHeavySound);

            StopGameScripts();

            gameOverUI.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        CheckEndGame();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PlayerWinScript()
    {
        StopGameScripts();
        enemy.GetComponent<Animator>().SetTrigger("Death");
        arena.GetComponentInChildren<AudioSource>().PlayOneShot(crowdHeavySound);
        player.GetComponent<PlayerCombat>().DrawWeapon();
        WinnerUI.SetActive(true);
    }

    void StopGameScripts()
    {
        player.GetComponent<PlayerMovement>().ResetAnimations();
        player.GetComponent<PlayerCombat>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.StopMoovingAnimations();
        enemyMovement.StopAttackingAnimations();
        enemyMovement.enabled = false;

        enemy.GetComponent<EnemyAttack>().enabled = false;
        GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>().enabled = false;

        healthBarUI.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void NextEnemy()
    {
        SceneManager.LoadScene(2);
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
    }

    // ------END------
}
