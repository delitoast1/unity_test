using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //important

//if you use this code you are contractually obligated to like the YT video
public class randomAItest : MonoBehaviour //don't forget to change the script name if you haven't
{
    private EnemyManager _manager;
    public LayerMask whatIsGround, whatIsPlayer;
    public float sightRange, attackRange;
    public Transform player;
    public NavMeshAgent agent;
    public float range; //radius of sphere

    public Transform centrePoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetManager(EnemyManager manager)
    {
        _manager = manager;
    }

    void Update()
    {
        if (player == null)
        {
            // Optional: try to auto-find the player
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        bool inSightRadius = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        bool hasLineOfSight = false;

        if (inSightRadius && player != null)
        {
            Vector3 eyePos = transform.position; // you can add + Vector3.up * 1.0f if you want eye height
            Vector3 directionToPlayer = player.position - eyePos;
            float distanceToPlayer = directionToPlayer.magnitude;

            Ray ray = new Ray(eyePos, directionToPlayer.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, distanceToPlayer))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    hasLineOfSight = true;
                    chase();
                    // Debug.Log("Chasing player");
                }
            }
        }

        // Patrol ONLY when not seeing player
        if (!hasLineOfSight)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 center = centrePoint != null ? centrePoint.position : transform.position;

                Vector3 point;
                if (RandomPoint(center, range, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                }
            }
        }
    }

    public void SetTarget(Transform playerTarget)
    {
        player = playerTarget;
    }

    void OnDestroy()
    {
        if (_manager != null)
        {
            _manager.NotifyEnemyDestroyed();
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void chase()
    {
        agent.SetDestination(player.position);
        //Debug.Log("bob is chasing player");
    }

}