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
    public float sightRange = 40f;
    public bool playerInSightRange;

    // Velocidades
    public float normalSpeed = 8f;
    public float chargeSpeed = 20f;

    // Knockback
    public float pushForce = 60f;
    public float liftForce = 3f;

    private bool charging = false;
    

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed; // empieza en velocidad normal
    }


    private void Update()
    {
        // Detectar jugador
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInSightRange && !charging)
        {
            // Patrulla en velocidad normal
            agent.speed = normalSpeed;
            Patroling();
        }
        else if (playerInSightRange && !charging)
        {
            // Detecta al jugador → inicia carga
            charging = true;
            agent.speed = chargeSpeed;
        }

        if (charging)
        {
            ChargePlayer();
        }
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

    private void ChargePlayer()
    {
        // Persigue directamente al jugador
        agent.SetDestination(player.position);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Si recibe daño mientras corre, reinicia
        if (charging)
        {
            charging = false;
            agent.speed = normalSpeed;
        }

        if (health <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (charging && other.CompareTag("Player"))
        {
            Health_and_Damage vida = other.GetComponent<Health_and_Damage>();
            New_CharacterController controller = other.GetComponent<New_CharacterController>();

            if (controller != null && vida != null)
            {
                // Knockback instantáneo diagonal
                controller.ApplyKnockback(transform.position, liftForce, pushForce);

                // Daño al jugador
                vida.RestarVida(15);
            }

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