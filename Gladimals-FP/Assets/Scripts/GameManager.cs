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
    bool endgame = false;
    

    void CheckEndGame()
    {
        if (player.GetComponent<PlayerHealth>().isDead() && !endgame){
            endgame = true;
            player.GetComponentInChildren<Animator>().SetTrigger("Death");
            player.GetComponent<PlayerCombat>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
            enemy.GetComponent<EnemyMovement>().StopMoovingAnimations();
            enemy.GetComponent<EnemyMovement>().enabled = false;
            enemy.GetComponent<EnemyAttack>().enabled = false;
        }
        
    }

    void FixedUpdate()
    {
        CheckEndGame();
    }
    // ------END------
}
