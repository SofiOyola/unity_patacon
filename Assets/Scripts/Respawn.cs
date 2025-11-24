using UnityEngine;

public class Respawn : MonoBehaviour
{
    [Header("Referencia al punto de respawn")]
    public Transform respawnPoint;

    private Health_and_Damage manager;

    public void Init(Health_and_Damage manager)
    {
        this.manager = manager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && respawnPoint != null)
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            Health_and_Damage health = other.GetComponent<Health_and_Damage>();

            if (controller != null)
            {
                controller.enabled = false;
                other.transform.position = respawnPoint.position;
                controller.enabled = true;

                // Restaurar vida
                if (health != null)
                {
                    health.vida = 100;
                    health.UpdatedVidaCounterUI();
                }

                Debug.Log("Jugador ha sido respawneado");
            }
            else
            {
                other.transform.position = respawnPoint.position;
                Debug.Log("Jugador sin characterController fue movido al respawn");
            }
        }
    }
}

