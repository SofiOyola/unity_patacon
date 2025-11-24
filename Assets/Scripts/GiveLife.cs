using UnityEngine;

public class GiveLife : MonoBehaviour
{
    public ParticleSystem brilloMagico;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health_and_Damage vidaPlayer = other.GetComponent<Health_and_Damage>();
            vidaPlayer.CurarVidaCompleta();

            // Activar el brillo mágico
            if (brilloMagico != null)
            {
                brilloMagico.transform.parent = null;  // Para que quede flotando
                brilloMagico.Play();
                Destroy(brilloMagico.gameObject, 2f); // Se limpia solo
            }

            Destroy(gameObject); // El polvo mágico desaparece
        }
    }
}

