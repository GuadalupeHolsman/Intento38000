using UnityEngine;

public class CableConnection : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject ventiladorFrio;
    public GameObject ventiladorCalor;
    public ParticleSystem particulasFrio;
    public ParticleSystem particulasCalor;
    public GameObject indicador;
    public BalanzaManager balanzaManager;

    [Header("Sonido")]
    public AudioClip sonidoConexion;      // <-- arrastrás el sonido desde el inspector
    private AudioSource audioSource;      // interno

    [Header("Velocidad de cambio")]
    public float tiempoCambio = 1f;

    private GameObject ventiladorConectado;
    private float tiempoActual = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();  // busca el componente AudioSource
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == ventiladorFrio || other.gameObject == ventiladorCalor)
        {
            ventiladorConectado = other.gameObject;
            tiempoActual = 0f;

            if (ventiladorConectado == ventiladorFrio && particulasFrio != null)
                particulasFrio.Play();
            else if (ventiladorConectado == ventiladorCalor && particulasCalor != null)
                particulasCalor.Play();

            // ▶️ Reproducir sonido
            if (sonidoConexion != null && audioSource != null)
                audioSource.PlayOneShot(sonidoConexion);

            Debug.Log("🔌 Cable conectado a: " + other.gameObject.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == ventiladorConectado)
        {
            if (ventiladorConectado == ventiladorFrio && particulasFrio != null)
                particulasFrio.Stop();
            else if (ventiladorConectado == ventiladorCalor && particulasCalor != null)
                particulasCalor.Stop();

            ventiladorConectado = null;
            Debug.Log("⚡ Cable desconectado de: " + other.gameObject.name);
        }
    }

    void Update()
    {
        if (ventiladorConectado != null)
        {
            tiempoActual += Time.deltaTime;

            if (tiempoActual >= tiempoCambio)
            {
                if (ventiladorConectado == ventiladorFrio)
                    balanzaManager.ReducirTemperatura();
                else if (ventiladorConectado == ventiladorCalor)
                    balanzaManager.AumentarTemperatura();

                tiempoActual = 0f;
            }
        }
    }
}