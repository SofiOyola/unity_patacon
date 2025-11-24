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
                brilloMagico.transform.parent = null;  
                brilloMagico.Play();
                Destroy(brilloMagico.gameObject, 2f); 
            }

            Destroy(gameObject); 
        }
    }
}

