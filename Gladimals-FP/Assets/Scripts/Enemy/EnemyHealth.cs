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


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponentInChildren<WeaponChanger>();
    }

    private void FixedUpdate()
    {
        CheckIfHited();
    }

    private void Update() {
        if(isDead() && !deathAnim){
            deathAnim = true;
            GetComponent<Animator>().SetTrigger("Death");
            GetComponent<EnemyMovement>().enabled = false;
            GetComponent<EnemyAttack>().enabled = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public bool isDead()
    {
        if (currentHealth <= 0) return true;
        else return false;
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
