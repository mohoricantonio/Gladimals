using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool swordCollision = false;
    public bool deathAnim = false;
    public HealthBar healthBar;
    private GameObject player;
    private WeaponChanger playerScript;
    public AudioClip crowdMediumSound;
    public AudioClip swordHitSound;
    private AudioSource arenaAudioSource;
    private AudioSource playerAudioSource;
    private Animator animator;

    public float timeBetweenDamageCombo = 1.0f;
    private float lastHitTime = 0f;
    private int attackComboCounter = 0;


    // Start is called before the first frame update
    void Start()
    {

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
        try{
            arenaAudioSource.PlayOneShot(crowdMediumSound, 0.4f);
        }
        catch{}
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
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
            lastHitTime = Time.time;
            attackComboCounter++;
            if (player.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(1).IsName("PowerSlash")
                && GetComponent<EnemyMovement>().canMove)
            {
                GetComponent<EnemyMovement>().CantMove(player.GetComponent<PlayerCombat>().HeavyAttackStunTime);
                GetComponent<EnemyMovement>().isAttacking = false;
            }
        }
    }

    public void NormalDamaged()
    {
        float now = Time.time;
        if((now - lastHitTime <= timeBetweenDamageCombo) && attackComboCounter == 2)
        {
            TakeDamage(30);
        }
        else if((now - lastHitTime <= timeBetweenDamageCombo) && attackComboCounter == 3)
        {
            TakeDamage(60);
            attackComboCounter = 0;
        }
        else
        {
            TakeDamage(20);
            playerScript.enemyIsHitable = false;
            GetComponent<EnemyMovement>().Hited();
            playerAudioSource.PlayOneShot(swordHitSound);
            attackComboCounter = 1;
        }
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
