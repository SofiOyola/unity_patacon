using UnityEngine;
using UnityEngine.AI;

public class ScriptVoladorEnemigo : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer, whatIsGround;

    // Vida del enemigo
    public float health = 1f;

    // Patrullar
    public Vector3 flyPoint;
    bool flyPointSet;
    public float flyPointRange = 5f;

    // Estados
    public float sightRange = 10f;
    public bool playerInSightRange;
    public bool retreating = false;

    // Sube y baja
    public float hoverAmplitude = 0.5f;
    public float hoverFrequency = 2f;
    private float startY;


    // Distancia de retirada
    public float retreatDistance = 5f;

    // Fuerza de empuje
    public float pushForce = 10f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        HoverMovement();

        // Detectar al jugador
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInSightRange && !retreating)
            Patroling();
        else if (playerInSightRange && !retreating)
            ChasePlayer();
        else if (retreating)
            RetreatFromPlayer();
    }

    private void HoverMovement()
    {
        float newY = startY + Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        Vector3 position = transform.position;
        position.y = newY;
        transform.position = position;
    }

    private void Patroling()
    {
        if (!flyPointSet) SearchFlyPoint();

        if (flyPointSet)
            agent.SetDestination(flyPoint);

        Vector3 distanceToFlyPoint = transform.position - flyPoint;

        if (distanceToFlyPoint.magnitude < 1f)
            flyPointSet = false;
    }

    private void SearchFlyPoint()
    {
        float randomZ = Random.Range(-flyPointRange, flyPointRange);
        float randomX = Random.Range(-flyPointRange, flyPointRange);
        float randomY = Random.Range(-flyPointRange, flyPointRange);

        flyPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z + randomZ);

        if (Physics.Raycast(flyPoint, -transform.up, 2f, whatIsGround))
            flyPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void RetreatFromPlayer()
    {
        Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
        Vector3 retreatPosition = transform.position + directionAwayFromPlayer * retreatDistance;
        agent.SetDestination(retreatPosition);

        if (Vector3.Distance(transform.position, player.position) >= retreatDistance)
        {
            retreating = false;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Daño al jugador
            other.GetComponent<PlayerLife>().PerderVida();
            // Empujar al jugador hacia atrás
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
            // Iniciar retirada
            retreating = true;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
