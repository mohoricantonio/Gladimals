using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject player;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float jumpRange;
    EnemyMovement enemyMovement;
    public bool isHeavyAttacking = false;
    [SerializeField]
    private bool isInRange = false;
    [SerializeField]
    private float AttackRange = 2f;
    [SerializeField]
    private bool isJumping = false;
    [SerializeField]
    private float time;

    private void Start() {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update() {
        if (isHeavyAttacking){
            heavyAttack(jumpRange);
        }
    }


    public void heavyAttack(float jumpRange){

        //if the player is not in range, move to the player
        if (!isInRange){
            isInRange = Vector3.Distance(transform.position, player.transform.position) < jumpRange;
            enemyMovement.MooveToPlayer(AttackRange);
        }
        //if the player is in range, jump
        else if (!isJumping){
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jump");
            isJumping = true;
        }

        // if the enemy is not grounded, move to the player in the air, and do the attack when the enemy is close enough
        else if (!enemyMovement.isGrounded){
            enemyMovement.MooveToPlayer(AttackRange);

            // if the enemy is close enough, do the attack
            if (Vector3.Distance(transform.position, player.transform.position) < AttackRange){
                Debug.Log("Attack");
            }
            time += Time.deltaTime;
        }
        else if (time > 0.1f){
            isHeavyAttacking = false;
            GetComponent<EnemyAI>().isAttacking = false;
            Debug.Log("End of attack");
        }
    }

    public void StartHeavyAttack(){
        isHeavyAttacking = true;
        isInRange = false;
        isJumping = false;
        time = 0f;
    }
}
