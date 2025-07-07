using UnityEngine;

public class CableConnection : MonoBehaviour
{
    public GameObject ventiladorFrio;
    public GameObject ventiladorCalor;
    public GameObject indicador;
    public BalanzaManager balanzaManager;

    [Header("Velocidad de cambio")]
    [Tooltip("Tiempo (en segundos) entre cada ajuste de temperatura")]
    public float tiempoCambio = 1f; // Ahora editable desde el Inspector

    private GameObject ventiladorConectado;
    private float tiempoActual = 0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == ventiladorFrio || other.gameObject == ventiladorCalor)
        {
            ventiladorConectado = other.gameObject;
            tiempoActual = 0f;
            Debug.Log("🔌 Cable conectado a: " + other.gameObject.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == ventiladorConectado)
        {
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
                {
                    balanzaManager.ReducirTemperatura();
                }
                else if (ventiladorConectado == ventiladorCalor)
                {
                    balanzaManager.AumentarTemperatura();
                }

                tiempoActual = 0f;
            }
        }
    }
}