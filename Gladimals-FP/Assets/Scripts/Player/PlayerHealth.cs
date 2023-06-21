using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool swordCollision = false;
    public HealthBar healthBar;
    public AudioClip crowdMediumSound;
    private AudioSource arenaAudioSource;
    private AudioSource playerAudioSource;
    public AudioClip catMeowSound;
    public AudioClip blockSound;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        GameObject arena = GameObject.FindGameObjectWithTag("Arena");
        if (arena != null)
        {
            arenaAudioSource = arena.GetComponent<AudioSource>();
        }
        playerAudioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        if(anim.GetBool("Blocking") == false)
        {
            playerAudioSource.PlayOneShot(catMeowSound);
            arenaAudioSource.PlayOneShot(crowdMediumSound);
            currentHealth -= damage;
            healthBar.setHealth(currentHealth);
        }
        else
        {
            playerAudioSource.PlayOneShot(blockSound);
            Debug.Log("Blocked!");
        }
    }

    public bool isDead(){
        return currentHealth <= 0;
    }
}
