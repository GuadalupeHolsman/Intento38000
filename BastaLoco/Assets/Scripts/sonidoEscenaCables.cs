using UnityEngine;

public class sonidoEscenaCables : MonoBehaviour
{
    public CableManager cableManager;

    public AudioSource audioFondo;           // AudioSource con sonido de fondo (loop)
    public AudioSource audioCompletado;      // AudioSource con sonido de completado (no en loop)

    private bool sonidoYaReproducido = false;

    void Start()
    {
        if (audioFondo != null)
            audioFondo.Play(); // Reproduce fondo desde el inicio
    }

    void Update()
    {
        if (cableManager != null && cableManager.escenaCompletada && !sonidoYaReproducido)
        {
            // Detener sonido de fondo
            if (audioFondo != null && audioFondo.isPlaying)
                audioFondo.Stop();

            // Reproducir sonido de completado
            if (audioCompletado != null)
                audioCompletado.Play();

            sonidoYaReproducido = true;
        }
    }
}
