using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool swordCollision = false;
    public bool deathAnim = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
}
