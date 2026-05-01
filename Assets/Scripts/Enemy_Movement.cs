using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Random;

public class Enemy_Movement : MonoBehaviour
{
    [Header("Stats")]
    public float TimeLastMoved;
    public float Agro;

    public float AttackRange;

    public Vector2 destination;


    [Header("State")]
    public string State;


    [Header("Refs")]
    [SerializeField] Transform Player;
    [SerializeField] LayerMask PlayerLayer;

    
    NavMeshAgent myNavMeshAgent;
    CircleCollider2D circleCollider2D;
    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        circleCollider2D = GetComponent<CircleCollider2D>();

        //to fix the collider idk
        circleCollider2D.enabled = true;
    }

    void Update()
    {
        MovingToPlayer();
    }

    void Attacking()
    {
    
    }

    void MovingToPlayer()
    {  
        destination = myNavMeshAgent.destination;

        Vector2 castDirection = Vector2.up; 

        float castDistance = AttackRange; 

        RaycastHit2D hit = Physics2D.CircleCast(destination, AttackRange, castDirection, castDistance, PlayerLayer);

        if (hit)
        {
            if(hit.collider.name == "Player")
            {
                Debug.Log(hit.collider.name);
            }
            else
            {  
                Debug.Log("Not Hiting PLaayer");

                myNavMeshAgent.SetDestination(Player.position);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(destination, AttackRange);
    }
}
