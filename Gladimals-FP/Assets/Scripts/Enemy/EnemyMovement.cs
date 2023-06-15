using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public bool goToPlayer = false;
    public Rigidbody rb;
    public float JumpAttackMovementSpeed = 3f;
    public float JumpAttackJumpForce = 9f;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if(goToPlayer){
            float distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * distanceToPlayer * JumpAttackMovementSpeed);
        }
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
}