using UnityEngine;

public class AudioDelay : MonoBehaviour
{
    public AudioSource audioSource; // Arrastrá el componente de AudioSource al Inspector
    public float delay = 1f; // Tiempo en segundos de espera

    void Start()
    {
        // Llama al método PlayAudio después de 'delay' segundos
        Invoke("PlayAudio", delay);
    }

    void PlayAudio()
    {
        audioSource.Play();
    }
}
