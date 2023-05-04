using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float time;
    public float timeBetweenAttacks = 20f;
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
            if (distanceToPlayer > 10f)
            {
                Run();
            }
            else if (distanceToPlayer < 5f)
            {
                RunBackwards();
            }
            else if (time < timeBetweenAttacks / 2)
            {
                StrafeRight();
            }
            else if (time >= timeBetweenAttacks / 2 && time < timeBetweenAttacks)
            {
                StrafeLeft();
            }
            else if (time >= timeBetweenAttacks)
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
}
