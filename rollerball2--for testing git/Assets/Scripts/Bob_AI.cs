using UnityEngine;
using UnityEngine.AI;

public class Bob_AI : MonoBehaviour
{
    private float stuckTimer = 0f;
    private float stuckTimeThresold = 3f;
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;
    private Rigidbody rb;
    public float health;

    //patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timebetweenAttacks;
    bool alreadyattacked;
    //public GameObject projectile;

    //states
    public float sightRange, attackRange;
    public bool playerinsight, playerinattackrange;

    private void Start()
    {
        rb.AddForce(Vector3.up * 10f);
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //check in sight and in range
        // Check if player is within sphere range
        bool inSightRadius = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        if (inSightRadius)
        {

            Vector3 eyePos = transform.position; // ground-level ray origin
            Vector3 directionToPlayer = player.position - eyePos;
            float distanceToPlayer = directionToPlayer.magnitude;

            // Draw the ray (color depends on whether it hits the player)
            Ray ray = new Ray(eyePos, directionToPlayer.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, distanceToPlayer))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    playerinsight = true;
                    Debug.Log("Raycast hit: " + hit.transform.name);
                }
                else
                {
                    playerinsight = false;
                    Debug.Log("Raycast hit: " + hit.transform.name);
                }
            }
            else
            {
                playerinsight = false;
                Debug.DrawRay(eyePos, directionToPlayer.normalized * distanceToPlayer, Color.gray);      //  Ray hits nothing
            }
        }
        else
        {
            playerinsight = false;
        }



        playerinattackrange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerinsight && !playerinattackrange)
        {
            Debug.Log("AI is patrolling...");
            patrolling();
        }
        if (playerinsight && !playerinattackrange)
        {
            chase();
        }
        if (playerinsight && playerinattackrange)
        {
            attack();
        }
    }
    private void patrolling()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            walkPointSet = false;
            stuckTimer = 0f; // Reset timer when goal reached
        }

        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.isStopped = false;
            agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude < 2f)
            {
                walkPointSet = false;
                stuckTimer = 0f; // Reset timer if arrived close enough
            }

            // 🧠 Better stuck detection:
            if (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
            {
                if (agent.velocity.sqrMagnitude < 0.01f)
                {
                    stuckTimer += Time.deltaTime;
                }
                else
                {
                    stuckTimer = Mathf.Max(0f, stuckTimer - Time.deltaTime * 2f); // Gracefully decay stuck timer
                }
            }

            if (stuckTimer >= stuckTimeThresold)
            {
                Debug.Log("Agent stuck for too long! Resetting patrol...");
                walkPointSet = false;
                stuckTimer = 0f;
            }
        }
    }

    private void SearchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 4f, whatIsGround))
        {
            walkPoint = hit.point; // Optional: snap to ground
            walkPointSet = true;
        }
    }

    private void chase()
    {
        agent.SetDestination(player.position);
    }

    private void attack()
    {
        //make sure enemy does't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        /* if (!alreadyattacked)
         {//attack code here

             Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
             rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
             rb.AddForce(transform.up * 8f, ForceMode.Impulse);
             alreadyattacked = true;
             Invoke(nameof(ResetAttack), timebetweenAttacks);
         }*/
    }

    private void ResetAttack()
    {
        alreadyattacked = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
