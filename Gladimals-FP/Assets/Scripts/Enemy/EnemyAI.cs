using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float time;
    [SerializeField]
    private float timeBetweenAttacks = 20f;
    public bool isAttacking = false;
    public Animator animator;


    // Update is called once per frame
    void Update()
    {
        /*
        if (!isAttacking){
            if (time < timeBetweenAttacks/2){
                GetComponent<EnemyMovement>().TurnArroundPlayerRight();
                animator.SetBool("isSideSteppingLeft", false);
                animator.SetBool("isSideSteppingRight", true);
            }
            else if (time >= timeBetweenAttacks/2 && time < timeBetweenAttacks){
                GetComponent<EnemyMovement>().TurnArroundPlayerLeft();
                animator.SetBool("isSideSteppingRight", false);
                animator.SetBool("isSideSteppingLeft", true);
            }
            else if (time >= timeBetweenAttacks){
                time = 0f;
                animator.SetBool("isSideSteppingLeft", false);
                animator.SetBool("isSideSteppingRight", false);
                GetComponent<EnemyAttack>().StartHeavyAttack();
                isAttacking = true;
            }
            time += Time.deltaTime;
        }*/
    }
}
