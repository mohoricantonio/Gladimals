using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    private float rotationSpeed = 5f;
    private float rotationDistanceToPlayer = 10f;
    public GameObject player;
    [SerializeField]
    public bool isGrounded = true;

    private void Start() {
    }
    

    void Update()
    {
        CheckGrounded();
    }


    private void FocusPlayer(){
        // Direction to the player
        Vector3 direction = player.transform.position - transform.position;

        // Rotate towards the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void MooveToPlayer(float distanceAim){
        FocusPlayer();
        // Move towards the player
        transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
    }

    private void BackFromPlayer(float distanceAim){
        FocusPlayer();
        // Move towards the player
        transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);

    }

    private void FleesFromPlayer(){
        // Face the direction opposite to the player
        Vector3 direction = transform.position - player.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Run away from the player
        transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
    }

    public void TurnArroundPlayerLeft(){
        FocusPlayer();

        // Turn around the player
        transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);

        StayAtDistanceFromPlayer(rotationDistanceToPlayer);
        
    }

    public void TurnArroundPlayerRight(){
        FocusPlayer();

        // Turn around the player
        transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);

        StayAtDistanceFromPlayer(rotationDistanceToPlayer);
    }

    private void StayAtDistanceFromPlayer(float distanceAim){
        float margin = 1f;
        // Stay at the same distance from the player
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance - margin > distanceAim){
            MooveToPlayer(distanceAim);
        }
        else if (distance + margin < distanceAim){
            BackFromPlayer(distanceAim);
        }
        else{
            FocusPlayer();
        }
    }

    private void CheckGrounded(){
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        float colliderRadius = collider.radius;

        // Check if the player is grounded
        isGrounded = Physics.CheckCapsule(
            collider.bounds.center,
            new Vector3(
                collider.bounds.center.x,
                collider.bounds.min.y,
                collider.bounds.center.z
            ),
            colliderRadius * .9f,
            LayerMask.GetMask("whatIsGround", "Entity")
        );
    }

}
