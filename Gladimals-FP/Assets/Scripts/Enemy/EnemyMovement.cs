using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public bool goToPlayer = false;
    public Rigidbody rb;
    public float JumpAttackMovementSpeed = 3f;
    public float JumpAttackJumpForce = 9f;
    public float StrafeTime = 0f;
    public bool strafeRight = false;
    public bool strafeLeft = false;
    public Animator animator;
    public bool isAttacking = false;
    public EnemyAttack enemyAttack;
    public int minDistanceToPlayer = 5;
    public int maxDistanceToPlayer = 10;

    private Transform fence;
    private bool hasRotated;
    private bool isCollidingWithFence;
    private float collisionCooldownTimer = 1.5f;
    private float collisionCooldown = 0f;
    private bool goFdAfterCollision = false;
    private bool plus = false;
    private string SideOfFence;

    private void Start() {
        fence = null;
        SideOfFence = "";
        isCollidingWithFence = false;
        hasRotated = false;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();
        if (SceneManager.GetActiveScene().name == "FinalFight")
        {
            animator.SetTrigger("Start");
        }
    }

    private void Update() {
        //Dealing with collision with fence
        if(collisionCooldown <0) collisionCooldown = 0;
        if(collisionCooldown > 0)
        {
            collisionCooldown -= Time.deltaTime;
        }
        //if there is no collision, enemy continues as it should
        if (!isCollidingWithFence && goFdAfterCollision == false && fence == null)
        {
            checkGoToPlayer();
            FocusTarget(GameObject.FindGameObjectWithTag("Player").transform);
            CkeckDistanceToPlayer();
        }
        //Dealing with collision with fence
        else if (!hasRotated && collisionCooldown == 0)
        {
            collisionCooldown = collisionCooldownTimer;
            StopMoovingAnimations();
            Vector3 directionToTarget = fence.transform.position - transform.position;

            Quaternion targetRotation;
            if (SideOfFence == "FrontCollider")
            {
                targetRotation = Quaternion.Euler(0, fence.transform.eulerAngles.y + 90, 0);
                plus = false;
            }
            else
            {
                targetRotation = Quaternion.Euler(0, fence.transform.eulerAngles.y - 90, 0);
                plus = true;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1);
            animator.SetBool("isRunning", true);
            hasRotated = true;
        }
        if (collisionCooldown == 0 && hasRotated == true && goFdAfterCollision == false)
        {
            goFdAfterCollision = true;
            StopMoovingAnimations();
            Quaternion targetRotation;
            if (plus == true)
                targetRotation = Quaternion.Euler(0, transform.eulerAngles.y - 90, 0);
            else
                targetRotation = Quaternion.Euler(0, transform.eulerAngles.y - 90, 0);
            transform.rotation = targetRotation;
            animator.SetBool("isRunning", true);
            StartCoroutine(ResetgoFdAfterCollision());
        }
    }

    private void FixedUpdate() {
        StrafeCheckChangeDirection();
        CheckIfAnimationIsFinished();
    }

    private IEnumerator ResetgoFdAfterCollision()
    {
        yield return new WaitForSeconds(1f);

        goFdAfterCollision = false;
        isCollidingWithFence = false;
        hasRotated = false;
        collisionCooldown = 0;
        StopMoovingAnimations();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Fence"))
        {
            SideOfFence = collision.gameObject.tag;
            isCollidingWithFence = true;
            fence = collision.transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Fence"))
        {
            fence = null;
            if (collisionCooldown == 0)
            {
                isCollidingWithFence = false;
                goFdAfterCollision = false;
                
                hasRotated = false;
                collisionCooldown = 0;
                StopMoovingAnimations();
            }
        }
    }

    public void FocusTarget(Transform target)
    {
        // Direction to the player
        Vector3 direction = target.position - transform.position;

        // Rotate towards the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Rotate the enemy
        transform.rotation = lookRotation;
    }

    public void Jump(){
        rb.AddForce(Vector3.up * JumpAttackJumpForce, ForceMode.Impulse);
        goToPlayer = true;
    }

    public void StopGoToPlayer(){
        goToPlayer = false;
    }

    public void checkGoToPlayer(){
        if(goToPlayer){
            float distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * distanceToPlayer * JumpAttackMovementSpeed);
        }
    }

    public void StrafeCheckChangeDirection(){
        if (StrafeTime <= 0){
            StrafeTime = Random.Range(5f, 10f);
            ChangeStrafeDirection();
        }
        else{
            StrafeTime -= Time.deltaTime;
        }
    }

    public void ChangeStrafeDirection(){
        if (strafeLeft){
            animator.SetBool("StrafeRight", true);
            animator.SetBool("StrafeLeft", false);
            strafeLeft = false;
            strafeRight = true;
        }
        else if (strafeRight) {
            animator.SetBool("StrafeLeft", true);
            animator.SetBool("StrafeRight", false);
            strafeLeft = true;
            strafeRight = false;
        }
        else{
            animator.SetBool("StrafeLeft", true);
            strafeLeft = true;
        }
    }

    public void StopAttacking()
    {
        isAttacking = false;
        LaunchStrafe();
    }

    private void LaunchStrafe()
    {
        StopMoovingAnimations();
        if (strafeLeft)
        {
            animator.SetBool("StrafeLeft", true);
        }
        else if (strafeRight)
        {
            animator.SetBool("StrafeRight", true);
        }
        else
        {
            animator.SetBool("StrafeLeft", true);
        }
    }

    public void Run(){
        StopMoovingAnimations();
        animator.SetBool("isRunning", true);
    }

    public void RunBackwards(){
        StopMoovingAnimations();
        animator.SetBool("isRunningBackward", true);
    }

    public void Hited()
    {
        animator.SetBool("hited", true);
    }

    public void CheckIfAnimationIsFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Hit") && stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("hited", false);
        }
    }

        public void StopMoovingAnimations(){
        animator.SetBool("StrafeLeft", false);
        animator.SetBool("StrafeRight", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isRunningBackward", false);
    }

    private void CkeckDistanceToPlayer()
    {
        if (isAttacking)
        {
            return;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        if (distanceToPlayer < minDistanceToPlayer)
        {
            RunBackwards();
        }
        else if (distanceToPlayer > maxDistanceToPlayer)
        {
            Run();
        }
        else
        {
            LaunchStrafe();
        }
        
    }

       internal void StopAttackingAnimations()
    {
        animator.SetBool("slashAttack", false);
        animator.SetBool("kickAttack", false);
        animator.SetBool("JumpAttack", false);
    }
}