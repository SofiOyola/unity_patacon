using UnityEngine;
using UnityEngine.AI;

public class ScriptEnemigoRojo : MonoBehaviour
{
public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    // Vida del enemigo
    public float health = 5f;

    // Patrullar
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 10f;

    // Estados
    public float sightRange = 60f;
    public bool playerInSightRange;

    // Carrera
    public float normalSpeed = 3.5f;
    public float chargeSpeed = 10f;
    public float retreatDistance = 3f;
    public float pushForce = 15f; // más fuerte que el helicóptero
    private bool charging = false;
    private bool preparingCharge = false;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInSightRange && !charging && !preparingCharge)
            Patroling();
        else if (playerInSightRange && !charging && !preparingCharge)
            PrepareCharge();
        else if (charging)
            ChargePlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void PrepareCharge()
    {
        preparingCharge = true;

        // Se echa hacia atrás un poquito
        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 retreatPos = transform.position + dir * retreatDistance;

        agent.SetDestination(retreatPos);

        // Cuando llega al punto de retroceso, inicia la carrera
        if (Vector3.Distance(transform.position, retreatPos) < 1f)
        {
            preparingCharge = false;
            charging = true;
            agent.speed = chargeSpeed;
        }
    }

    private void ChargePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Si recibe daño mientras corre, reinicia
        if (charging || preparingCharge)
        {
            charging = false;
            preparingCharge = false;
            agent.speed = normalSpeed;
        }

        if (health <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (charging && other.CompareTag("Player"))
        {
            // Empuje físico al jugador
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 pushDir = (other.transform.position - transform.position).normalized;
                rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
            }

            // Daño al jugador
            other.GetComponent<PlayerLife>().PerderVida();

            // Reinicia después del impacto
            charging = false;
            agent.speed = normalSpeed;
        }

        // Si choca con algo mientras corre → reinicia
        if (charging && !other.CompareTag("Player"))
        {
            charging = false;
            agent.speed = normalSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}