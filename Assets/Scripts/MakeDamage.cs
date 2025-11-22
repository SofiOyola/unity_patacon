using UnityEngine;

public class MakeDamage : MonoBehaviour
{
    public int cantidad = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health_and_Damage>().RestarVida(cantidad);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health_and_Damage>().RestarVida(cantidad);
        }
    }
}

