using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Animator animator;
    public EnemyMovement enemyMovement;
    public GameObject player;
    public float time;
    public float mqxCycleTime = 10f;
    public float minCycleTime = 2f;
    public Hashtable cycle = new Hashtable();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cycle.Add("CheckDistance", Random.Range(minCycleTime, mqxCycleTime));
        cycle.Add("StrafeLeft", Random.Range(minCycleTime, mqxCycleTime) + (float)cycle["CheckDistance"]);
        cycle.Add("StrafeRight", Random.Range(minCycleTime, mqxCycleTime) + (float)cycle["StrafeLeft"]);

    }

    void Update()
    {
        enemyMovement.FocusTarget(player.transform);
        bool isAttacking = animator.GetBool("JumpAttack");
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

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
            Debug.Log("animationNormalizedTime: " + animationNormalizedTime);
            if (animationIsJumpAttack && animationNormalizedTime >= 1f)
            {
                animator.SetBool("JumpAttack", false);
                Debug.Log("JumpAttack ended");
                updateCycleTime();
            }
            time = 0f;
        }

    }

    private void JumpAttack()
    {
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
        cycle["StrafeLeft"] = Random.Range(minCycleTime, mqxCycleTime) + (float)cycle["CheckDistance"];
        cycle["StrafeRight"] = Random.Range(minCycleTime, mqxCycleTime) + (float)cycle["StrafeLeft"];

        Debug.Log("CheckDistance: " + cycle["CheckDistance"]);
        Debug.Log("StrafeLeft: " + cycle["StrafeLeft"]);
        Debug.Log("StrafeRight: " + cycle["StrafeRight"]);
    }
}
