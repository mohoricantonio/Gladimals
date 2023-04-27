using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject player;
    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private float jumpRange = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void heavyAttack(float jumpRange){
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        // Jump
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * distanceFromPlayer * jumpForce);

        // TODO: Make damage
    
    }
}
