using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public int vidas = 3; // inicial con 3 "polvo_magico"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PerderVida();
        }
    }

    public void PerderVida()
    {
        vidas--;

        Debug.Log("Vidas restantes: " + vidas);

        if (vidas <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log("¡Has perdido!");
        // Cambia la escena o reinicia el nivel
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
