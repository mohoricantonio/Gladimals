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



    // Update is called once per frame
    void Update()
    {
        if (!isAttacking){
            if (time >= timeBetweenAttacks){
                time = 0f;
                isAttacking = true;
                GetComponent<EnemyAttack>().StartHeavyAttack();
            }
            else{
                time += Time.deltaTime;
            }
            if (time >= timeBetweenAttacks/2 && time < timeBetweenAttacks){
                GetComponent<EnemyMovement>().TurnArroundPlayerLeft();
            }
            else if (time < timeBetweenAttacks/2){
                GetComponent<EnemyMovement>().TurnArroundPlayerRight();
            }
        }
    }
}
