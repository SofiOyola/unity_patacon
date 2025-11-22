using UnityEngine;

public class HachaDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<ScriptEnemigo>().TakeDamage(damage);
        }
    }
}

