using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ContinuosGame : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Cuando termine el video, carga la escena del juego
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene("Escenario1"); // nombre exacto de tu escena
    }

}
