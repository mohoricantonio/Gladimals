using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    private float rotationSpeed = 5f;
    private float minDistanceToPlayer = 2f;
    private float rotationDistanceToPlayer = 10f;
    public GameObject player;
    
    void Start() {
        heavyAttack(10f);
    }

    void Update()
    {
        TurnArroundPlayerLeft();
    }

    private void FocusPlayer(){
        // Direction to the player
        Vector3 direction = player.transform.position - transform.position;

        // Rotate towards the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void MooveToPlayer(float distanceAim){
        float distance = Vector3.Distance(transform.position, player.transform.position);
        while (distance > distanceAim){
            FocusPlayer();

            // Move towards the player
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);

            distance = Vector3.Distance(transform.position, player.transform.position);
        }
    }

    private void FleesFromPlayer(){
        // Face the direction opposite to the player
        Vector3 direction = transform.position - player.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Run away from the player
        transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
    }

    private void TurnArroundPlayerLeft(){
        FocusPlayer();

        // Turn around the player
        transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);

        StayAtDistanceFromPlayer(rotationDistanceToPlayer);
        
    }

    private void TurnArroundPlayerRight(){
        FocusPlayer();

        // Turn around the player
        transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);

        StayAtDistanceFromPlayer(rotationDistanceToPlayer);
    }

    private void StayAtDistanceFromPlayer(float distanceAim){
        // Stay at the same distance from the player
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > distanceAim){
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
        }
        else if (distance < distanceAim){
            transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
        }
    }

    private void heavyAttack(float jumpRange){
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceFromPlayer > jumpRange){
            MooveToPlayer(jumpRange);
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        }
        // Jump
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 1000f);
        MooveToPlayer(minDistanceToPlayer);

        // TODO: Make damage
    
    }

    private void RandomAction(){
        // Randomly choose an action
        int action = Random.Range(1, 4);

        // If the action is 1, move towards the player
        if (action == 1){
            MooveToPlayer(minDistanceToPlayer);
        }
        // If the action is 2, run away from the player
        else if (action == 2){
            FleesFromPlayer();
        }
        // If the action is 3, turn around the player to the left
        else if (action == 3){
            TurnArroundPlayerLeft();
        }
        // If the action is 4, turn around the player to the right
        else if (action == 4){
            TurnArroundPlayerRight();
        }
    }



/* useless because we want the enemy to follow the player

    [SerializeField]
    private float detectionRadius = 10f;

    [SerializeField]
    private float detectionAngle = 45f;

    private void PlayerDetection(){
        // Get all colliders within detection radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        // Check if any of the colliders are the player
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                // Check if the player is within the detection angle
                Vector3 dedectionDirection = collider.transform.position - transform.position;
                float angle = Vector3.Angle(dedectionDirection, transform.forward);
                if (angle < detectionAngle){
                    Debug.Log("Player Detected");
                }
            }
        }
    }
*/
}
