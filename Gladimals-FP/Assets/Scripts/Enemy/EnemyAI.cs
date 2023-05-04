using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float time;
    public float mqxCycleTime = 10f;
    public float minCycleTime = 2f;
    public Hashtable cycle = new Hashtable();

    private void Awake()
    {
        cycle.Add("CheckDistance", Random.Range(minCycleTime, mqxCycleTime));
        cycle.Add("StrafeLeft", Random.Range(minCycleTime, mqxCycleTime));
        cycle.Add("StrafeRight", Random.Range(minCycleTime, mqxCycleTime));

    }


    public Animator animator;
    public EnemyMovement enemyMovement;
    public Transform player;


    // Update is called once per frame
    void Update()
    {
        bool isAttacking = animator.GetBool("JumpAttack");
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            if (time < (float)cycle["CheckDistance"])

            {
                if (distanceToPlayer > 10f)
                {
                    Run();
                }
                else if (distanceToPlayer < 5f)
                {
                    RunBackwards();
                }
            }
            else if (time >= (float)cycle["CheckDistance"] && time < (float)cycle["StrafeLeft"])
            {
                StrafeRight();
            }
            else if (time >= (float)cycle["StrafeLeft"] && time < (float)cycle["StrafeRight"])
            {
                StrafeLeft();
            }
            else if (time >= (float)cycle["StrafeRight"])
            {
                JumpAttack();
            }
            time += Time.deltaTime;
        }
        else
        {
            bool animationIsJumpAttack = animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack");
            float animationNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animationIsJumpAttack && animationNormalizedTime >= 1f)
            {
                animator.SetBool("JumpAttack", false);
                Debug.Log("JumpAttack ended");
                updateCycleTime();
            }
        }

        enemyMovement.FocusTarget(player);
    }

    private void JumpAttack()
    {
        time = 0f;
        StopMooving();
        animator.SetBool("JumpAttack", true);
    }

    private void StrafeLeft()
    {
        StopMooving();
        animator.SetBool("isSideSteppingLeft", true);
    }

    private void StrafeRight()
    {
        StopMooving();
        animator.SetBool("isSideSteppingRight", true);
    }

    private void Run()
    {
        StopMooving();
        animator.SetBool("isRunning", true);
    }

    private void RunBackwards()
    {
        StopMooving();
        animator.SetBool("isRunningBackward", true);
    }

    private void StopMooving()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isRunningBackward", false);
        animator.SetBool("isSideSteppingLeft", false);
        animator.SetBool("isSideSteppingRight", false);
    }

    private void updateCycleTime()
    {
        cycle["CheckDistance"] = Random.Range(minCycleTime, mqxCycleTime);
        cycle["StrafeLeft"] = Random.Range(minCycleTime, mqxCycleTime);
        cycle["StrafeRight"] = Random.Range(minCycleTime, mqxCycleTime);

        Debug.Log("CheckDistance: " + cycle["CheckDistance"]);
        Debug.Log("StrafeLeft: " + cycle["StrafeLeft"]);
        Debug.Log("StrafeRight: " + cycle["StrafeRight"]);
    }
}
