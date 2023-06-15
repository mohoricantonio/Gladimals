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

    private void Start() {
        enemyMovement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate() {
        if (timeToAttack > 0)
        {
            timeToAttack -= Time.deltaTime;
        }
        if (timeToAttack <= 0)
        {
            timeToAttack = Random.Range(5, 10);
            Attack();
        }

        CheckIfAnimationIsFinished();
    }

    public void Attack(){
        enemyMovement.isAttacking = true;
        int attackProba = Random.Range(0, 100);
        Debug.Log(attackProba);

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

    public void CheckIfAnimationIsFinished(){
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("SlashAttack") && stateInfo.normalizedTime >= 1.0f){
            animator.SetBool("slashAttack", false);
            enemyMovement.StopAttacking();
        }

        else if(stateInfo.IsName("KickAttack") && stateInfo.normalizedTime >= 1.0f){
            animator.SetBool("kickAttack", false);
            enemyMovement.StopAttacking();
            if (isComboKickJump){
                isComboKickJump = false;
                animator.SetBool("JumpAttack", true);
                enemyMovement.isAttacking = true;
            }
        }

        else if(stateInfo.IsName("JumpAttack") && stateInfo.normalizedTime >= 1.0f){
            animator.SetBool("JumpAttack", false);
            enemyMovement.StopAttacking();
        }
    }

    public void KickAnimationEvent(){
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer < attackRange)
        {
            player.GetComponent<Rigidbody>().AddForce(transform.forward * kickBackForce + Vector3.up * kickUpForce);
            player.GetComponent<PlayerMovement>().cantMoove(stunTime);
        }
    }
}
