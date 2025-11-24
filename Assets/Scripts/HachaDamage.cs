using UnityEngine;

public class HachaDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var e1 = other.GetComponent<ScriptEnemigo>();
            var e2 = other.GetComponent<ScriptEnemigoRojo>();
            var e3 = other.GetComponent<ScriptVoladorEnemigo>();

            if (e1 != null) e1.TakeDamage(damage);
            if (e2 != null) e2.TakeDamage(damage);
            if (e3 != null) e3.TakeDamage(damage);
        }

    }
}

