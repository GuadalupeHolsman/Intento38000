using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenaPorZona : MonoBehaviour
{
    public string nombreEscena;
    public AudioClip sonido;
    
    private AudioSource audioSource;
    private bool escenaCambiada = false;

    void Start()
    {
        // Agrega un AudioSource si no hay uno
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.clip = sonido;
    }

    void OnMouseDown()
    {
        ReproducirSonidoYCambiarEscena();
    }

    void OnMouseUpAsButton()
    {
        ReproducirSonidoYCambiarEscena();
    }

    void ReproducirSonidoYCambiarEscena()
    {
        if (!escenaCambiada && sonido != null)
        {
            escenaCambiada = true;
            audioSource.Play();
            Invoke("CambiarEscena", sonido.length);
        }
        else if (!escenaCambiada)
        {
            CambiarEscena();
        }
    }

    void CambiarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}