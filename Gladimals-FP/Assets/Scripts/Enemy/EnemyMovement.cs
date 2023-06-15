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

    private void Start() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        checkGoToPlayer();
        FocusTarget(GameObject.FindGameObjectWithTag("Player").transform);
        CheckIsAttacking();
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

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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

    public void CheckIsAttacking(){
        if (isAttacking){
            animator.SetBool("StrafeLeft", false);
            animator.SetBool("StrafeRight", false);
        }
    }

    public void StopAttacking(){
        isAttacking = false;
        if (strafeLeft){
            animator.SetBool("StrafeLeft", true);
        }
        else if (strafeRight){
            animator.SetBool("StrafeRight", true);
        }
        else{
            animator.SetBool("StrafeLeft", true);
        }
    }
}