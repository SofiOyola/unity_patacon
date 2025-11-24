using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAxeCollisions : MonoBehaviour
{
    public Rigidbody rb;
    public Axe axe;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        axe.CollisionOccured();
        rb.useGravity = false;
        rb.isKinematic = true;
        AddConstraints();

        // Si colisiona con un enemigo
        if (collision.collider.CompareTag("Enemy"))
        {
            GameObject enemigo = collision.collider.gameObject;

            CongelarEnemigo(enemigo);

            transform.position = collision.contacts[0].point;
            transform.SetParent(null);

            axe.lastEnemyHit = enemigo;
        }
    }

    void CongelarEnemigo(GameObject enemigo)
    {
        var agent = enemigo.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        var rbEnemigo = enemigo.GetComponent<Rigidbody>();
        if (rbEnemigo != null)
        {
            rbEnemigo.linearVelocity = Vector3.zero;
            rbEnemigo.angularVelocity = Vector3.zero;
            rbEnemigo.isKinematic = true;  
        }

        var anim = enemigo.GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.enabled = false;
        }
        var e1 = enemigo.GetComponent<ScriptEnemigo>();
        if (e1 != null) e1.enabled = false;

        var e2 = enemigo.GetComponent<ScriptEnemigoRojo>();
        if (e2 != null) e2.enabled = false;

        var e3 = enemigo.GetComponent<ScriptVoladorEnemigo>();
        if (e3 != null) e3.enabled = false;
    }


    public void AddConstraints()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }

    public void RemoveConstraints()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
}
