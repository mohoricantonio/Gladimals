using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeToAttack = 0;
    public Animator animator;
    public EnemyMovement enemyMovement;
    public bool isComboKickJump = false;
    public GameObject player;
    public float attackRange = 2f;
    public float kickBackForce = 600f;
    public float kickUpForce = 200f;
    public int stunTime = 2;
    public bool isAttacking = false;
    public int attackProba = 0;
    public float lastAttackCooldown = 0;
    public float lastAttackCooldownMax = 3;

    private void Start() {
        enemyMovement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate() {
        if (timeToAttack > 0)
        {
            timeToAttack -= Time.deltaTime;
            lastAttackCooldown -= Time.deltaTime;
            CheckIfPlayerIsClose();
        }
        if (timeToAttack <= 0)
        {
            Attack();
        }

        CheckIfAnimationIsFinished();

    }

    public bool PlayerIsClose()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < attackRange)
        {
            return true;
        }
        return false;
    }

    public void Attack(){
        if (!isAttacking){
            attackProba = Random.Range(0, 100);
            isAttacking = true;
        }
        
        if (!PlayerIsClose() && (attackProba < 70 || attackProba >= 90))
        {
            enemyMovement.isAttacking = true;
            enemyMovement.Run();
        }
        else{
            timeToAttack = Random.Range(5, 10);
            enemyMovement.StopMoovingAnimations();

            if(attackProba < 35){
                animator.SetBool("slashAttack", true);
            }
            else if(attackProba >= 35 && attackProba < 70){
                animator.SetBool("kickAttack", true);
            }
            else if(attackProba >= 70 && attackProba < 90){
                animator.SetBool("JumpAttack", true);
            }
            else if (attackProba >= 90){
                animator.SetBool("kickAttack", true);
                isComboKickJump = true;
            }
        }
    }

    public void CheckIfAnimationIsFinished(){
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("SlashAttack") && stateInfo.normalizedTime >= 1.0f){
            animator.SetBool("slashAttack", false);
            enemyMovement.StopAttacking();
            isAttacking = false;
            lastAttackCooldown = lastAttackCooldownMax;
        }

        else if(stateInfo.IsName("KickAttack") && stateInfo.normalizedTime >= 1.0f){
            animator.SetBool("kickAttack", false);
            if (isComboKickJump){
                animator.SetBool("JumpAttack", true);
                enemyMovement.isAttacking = true;
            }
            else{
                enemyMovement.StopAttacking();
                isAttacking = false;
                lastAttackCooldown = lastAttackCooldownMax;
            }
        }

        else if(stateInfo.IsName("JumpAttack") && stateInfo.normalizedTime >= 1.0f){
            animator.SetBool("JumpAttack", false);
            enemyMovement.StopAttacking();
            isAttacking = false;
            isComboKickJump = false;
            lastAttackCooldown = lastAttackCooldownMax;
        }

        animator.SetBool("hited", false);

    }

    public void KickAnimationEvent(){
        if (PlayerIsClose())
        {
            player.GetComponent<Rigidbody>().AddForce(transform.forward * kickBackForce + Vector3.up * kickUpForce);
            player.GetComponent<PlayerMovement>().cantMoove(stunTime);
            player.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }

    public void CheckIfPlayerIsClose(){
        if (PlayerIsClose() && !isAttacking && lastAttackCooldown <=0)
        {
            enemyMovement.StopMoovingAnimations();
            animator.SetBool("kickAttack", true);
            isComboKickJump = true;
            isAttacking = true;
            enemyMovement.isAttacking = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            GetComponent<EnemyHealth>().swordCollision = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            GetComponent<EnemyHealth>().swordCollision = false;
        }
    }

    public void damageAnimEvent(){
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth.swordCollision)
        {
            playerHealth.TakeDamage(20);
        }
    }
}
