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

    // Start is called before the first frame update
    void Start()
    {
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
        playerAudioSource.PlayOneShot(catMeowSound);
        arenaAudioSource.PlayOneShot(crowdMediumSound);
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
    }

    public bool isDead(){
        return currentHealth <= 0;
    }
}
