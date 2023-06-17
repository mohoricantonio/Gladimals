using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    private float rotationSpeed = 5f;
    private float rotationDistanceToPlayer = 10f;
    [SerializeField]
    public bool isGrounded = true;


    public void FocusTarget(Transform target)
    {
        // Direction to the player
        Vector3 direction = target.position - transform.position;

        // Rotate towards the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}