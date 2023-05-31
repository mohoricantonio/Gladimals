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
    public float attackRange = 2f;
    public int comboKickJumpState = 0;

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
        bool isAttacking = animator.GetBool("JumpAttack") || animator.GetBool("kickAttack") || animator.GetBool("slashAttack");
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (!isAttacking)
        {
            if(AttackIfPlayerIsClose(distanceToPlayer))
            {
                return;
            }
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
                else
                {
                    StopMoovingAnimations();
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
            AnimatorStateInfo currentAnnimInfo = animator.GetCurrentAnimatorStateInfo(0);
            bool animationIsJumpAttack = currentAnnimInfo.IsName("JumpAttack");
            bool animationIsKickAttack = currentAnnimInfo.IsName("KickAttack");
            bool animationIsSlashAttack = currentAnnimInfo.IsName("SlashAttack");
            float animationNormalizedTime = currentAnnimInfo.normalizedTime;
            //Debug.Log("animationNormalizedTime: " + animationNormalizedTime);

            if (comboKickJumpState == 1){
                if (animationIsKickAttack && animationNormalizedTime >= 1f){
                    animator.SetBool("kickAttack", false);
                    JumpAttack();
                    comboKickJumpState = 0;
                    Debug.Log("Second attack started");
                }
            }
            else{
                if (animationIsJumpAttack && animationNormalizedTime >= 1f)
                {
                    animator.SetBool("JumpAttack", false);
                    Debug.Log("JumpAttack ended");
                    updateCycleTime();
                }
                else if (animationIsKickAttack && animationNormalizedTime >= 1f)
                {
                    animator.SetBool("kickAttack", false);
                    Debug.Log("kickAttack ended");
                    updateCycleTime();
                }
                else if (animationIsSlashAttack && animationNormalizedTime >= 1f)
                {
                    animator.SetBool("slashAttack", false);
                    Debug.Log("slashAttack ended");
                    updateCycleTime();
                }
                else
                {
                    //Debug.Log("animation is not over");
                }
            }
            time = 0f;
        }
        
    }

    private bool AttackIfPlayerIsClose(float distanceToPlayer)
    {
        if (distanceToPlayer < attackRange)
        {
            ComboKickJumpAttack();
            return true;
        }
        return false;
    }

    private void ComboKickJumpAttack()
    {
        KickAttack();
        comboKickJumpState = 1;
        Debug.Log("ComboKickJumpAttack started");
    }

    private void JumpAttack()
    {
        StopMoovingAnimations();
        animator.SetBool("JumpAttack", true);
    }

    private void KickAttack()
    {
        StopMoovingAnimations();
        animator.SetBool("kickAttack", true);
    }

    private void SlashAttack()
    {
        StopMoovingAnimations();
        animator.SetBool("slashAttack", true);
    }

    private void StrafeLeft()
    {
        StopMoovingAnimations();
        animator.SetBool("isSideSteppingLeft", true);
    }

    private void StrafeRight()
    {
        StopMoovingAnimations();
        animator.SetBool("isSideSteppingRight", true);
    }

    private void Run()
    {
        StopMoovingAnimations();
        animator.SetBool("isRunning", true);
    }

    private void RunBackwards()
    {
        StopMoovingAnimations();
        animator.SetBool("isRunningBackward", true);
    }

    private void StopMoovingAnimations()
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

        //Debug.Log("CheckDistance: " + cycle["CheckDistance"]);
        //Debug.Log("StrafeLeft: " + cycle["StrafeLeft"]);
        //Debug.Log("StrafeRight: " + cycle["StrafeRight"]);
    }
}
