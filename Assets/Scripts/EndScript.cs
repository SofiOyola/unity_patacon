using UnityEngine;
using UnityEngine.Video;

public class EndScript : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Reproducir el video automáticamente
        videoPlayer.Play();

        // Cuando termine el video, cerrar el juego
        videoPlayer.loopPointReached += EndVideo;
    }

    void EndVideo(VideoPlayer vp)
    {
        Application.Quit(); // Finaliza el juego en el build

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Detiene el modo Play en el editor
        #endif
    }

}
