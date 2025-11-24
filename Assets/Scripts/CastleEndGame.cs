using UnityEngine;
using UnityEngine.SceneManagement;

public class CastleEndGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Fin");
        }
    }

}
