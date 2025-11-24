using UnityEngine;
using UnityEngine.AI;

public class ScriptVoladorEnemigo : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer;

    // Vida del enemigo
    public float health = 3f;

    // Patrullar
    public Vector3 flyPoint;
    bool flyPointSet;
    public float flyPointRange = 10f;

    // Estados
    public float sightRange = 100f;
    public bool playerInSightRange;
    public bool retreating = false;

    // Sube y baja
    public float hoverAmplitude = 0.5f;
    public float hoverFrequency = 2f;
    private float baseY;

    // Vuelo
    public float minHeight = 3f;
    public float maxHeight = 8f;
    private float currentHeight;
    private float heightChangeInterval = 5f;
    private float heightTimer;


    // Distancia de retirada
    public float retreatDistance = 35f;

    // Fuerza de empuje
    public float pushForce = 8f;
    public float liftForce = 2.5f;

private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        // El agente no debe rotar ni usar eje Y del NavMesh
        agent.updateUpAxis = false;
        agent.updateRotation = false;

        baseY = transform.position.y;
    }

    private void Update()
    {
        HoverMovement();

        heightTimer += Time.deltaTime;
        if (heightTimer >= heightChangeInterval)
        {
            currentHeight = Random.Range(minHeight, maxHeight);
            heightTimer = 0f;
        }

        // Detectar al jugador
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInSightRange && !retreating)
            Patrol();
        else if (playerInSightRange && !retreating)
            ChasePlayer();
        else if (retreating)
            RetreatFromPlayer();
    }

    // --- Movimiento de vuelo (hover) ---
    private void HoverMovement()
    {
        float newY = baseY + currentHeight + Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }


    // --- Patrulla aérea ---
    private void Patrol()
    {
        if (!flyPointSet) SearchFlyPoint();

        if (flyPointSet && agent.isOnNavMesh)
            agent.SetDestination(flyPoint);

        Vector3 distanceToFlyPoint = transform.position - flyPoint;

        if (distanceToFlyPoint.magnitude < 1f)
            flyPointSet = false;
    }

    private void SearchFlyPoint()
    {
        float randomZ = Random.Range(-flyPointRange, flyPointRange);
        float randomX = Random.Range(-flyPointRange, flyPointRange);

        // El destino se calcula en el piso fantasma, luego se ajusta la altura
        flyPoint = new Vector3(transform.position.x + randomX, baseY, transform.position.z + randomZ);
        flyPointSet = true;
    }

    // --- Persecución del jugador ---
    private void ChasePlayer()
    {
        if (agent.isOnNavMesh)
        {
            Vector3 groundTarget = new Vector3(player.position.x, baseY, player.position.z);
            agent.SetDestination(groundTarget);
        }
    }

    // --- Retirada ---
    private void RetreatFromPlayer()
    {
        if (agent.isOnNavMesh)
        {
            PerformRetreat();
        }
    }
    private void PerformRetreat()
    {
        Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
        Vector3 retreatPosition = transform.position + directionAwayFromPlayer * retreatDistance;
        agent.SetDestination(new Vector3(retreatPosition.x, baseY, retreatPosition.z));

        if (Vector3.Distance(transform.position, player.position) >= retreatDistance)
            retreating = false;
    }

    // --- Daño y muerte ---
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health_and_Damage vida = other.GetComponent<Health_and_Damage>();
            New_CharacterController controller = other.GetComponent<New_CharacterController>();

            if (controller != null && vida != null)
            {
                // Knockback instantáneo diagonal
                controller.ApplyKnockback(transform.position, liftForce, pushForce); 

                // Aplicar daño
                vida.RestarVida(10);
            }

            retreating = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}