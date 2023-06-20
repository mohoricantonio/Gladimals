using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool swordCollision = false;
    public bool deathAnim = false;
    private GameObject player;
    private WeaponChanger playerScript;
    public AudioClip crowdMediumSound;
    public AudioClip swordHitSound;
    private AudioSource arenaAudioSource;
    private AudioSource playerAudioSource;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponentInChildren<WeaponChanger>();
        GameObject arena = GameObject.FindGameObjectWithTag("Arena");
        if (arena != null)
        {
            arenaAudioSource = arena.GetComponent<AudioSource>();
        }
        if (player != null)
        {
            playerAudioSource = player.GetComponent<AudioSource>();
        }
    }

    private void FixedUpdate()
    {
        CheckIfHited();
    }

    private void Update() {
        if(isDead() && !deathAnim){
            deathAnim = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().PlayerWinScript();
        }
    }

    public void TakeDamage(int damage)
    {
        arenaAudioSource.PlayOneShot(crowdMediumSound);
        currentHealth -= damage;
    }

    public bool isDead()
    {
        return currentHealth <= 0;
    }

    private void CheckIfHited()
    {
        if (playerScript.enemyIsHitable && swordCollision)
        {
            NormalDamaged();
        }
    }

    public void NormalDamaged()
    {
        TakeDamage(20);
        playerScript.enemyIsHitable = false;
        GetComponent<EnemyMovement>().Hited();
        playerAudioSource.PlayOneShot(swordHitSound);
    }

    public void HighDamaged()
    {
        if (swordCollision)
        {
            TakeDamage(40);
            GetComponent<EnemyMovement>().Hited();
        }
    }
}
