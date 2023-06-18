using UnityEngine;

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

    private void Start() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Update() {
        checkGoToPlayer();
        FocusTarget(GameObject.FindGameObjectWithTag("Player").transform);
        CkeckDistanceToPlayer();
    }

    private void FixedUpdate() {
        StrafeCheckChangeDirection();
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