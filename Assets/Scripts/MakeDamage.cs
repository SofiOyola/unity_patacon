using UnityEngine;

public class MakeDamage : MonoBehaviour
{
    public int cantidad = 10;

    private void OnTriggerEnter(Collider other)
    {
        Health_and_Damage vida = other.GetComponentInParent<Health_and_Damage>();

        if (vida != null)
        {
            vida.RestarVida(cantidad);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Health_and_Damage vida = other.GetComponentInParent<Health_and_Damage>();

        if (vida != null)
        {
            vida.RestarVida(cantidad);
        }
    }
}
