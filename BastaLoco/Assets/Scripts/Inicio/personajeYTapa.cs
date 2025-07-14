using UnityEngine;
using System.Collections; // Importante para usar corutinas

public class personajeYTapa : MonoBehaviour
{
    [Header("Referencias")]
    public SpriteRenderer personaje;
    public Transform posicionAsomado;
    public Transform posicionEscondido;

    public Transform tapa;
    public Transform posicionFinalTapa;

    [Header("Tiempos y Velocidades")]
    public float velocidadPersonaje = 2f;
    public float tiempoVisible = 2f;
    public float velocidadTapa = 2f;

    private bool personajeApareciendo = false;
    private bool personajeDesapareciendo = false;
    private bool moverTapa = false;

    void Start()
    {
        if (gameManager.instance != null && gameManager.instance.personajeYaAparecio)
        {
            // Ya apareció antes, lo dejamos escondido y movemos la tapa
            if (personaje != null)
            {
                personaje.enabled = false;
            }

            if (tapa != null && posicionFinalTapa != null)
            {
                tapa.position = posicionFinalTapa.position;
            }
        }
        else
        {
            // Primera vez: comenzamos el ciclo después de 5 segundos
            if (personaje != null && posicionEscondido != null)
            {
                personaje.transform.position = posicionEscondido.position;
                personaje.enabled = true;
            }

            StartCoroutine(EsperarYComenzar());
        }
    }

    IEnumerator EsperarYComenzar()
    {
        yield return new WaitForSeconds(5f); // Espera 5 segundos
        personajeApareciendo = true;
    }

    void Update()
    {
        if (personajeApareciendo && personaje != null)
        {
            personaje.transform.position = Vector3.MoveTowards(personaje.transform.position, posicionAsomado.position, velocidadPersonaje * Time.deltaTime);

            if (Vector3.Distance(personaje.transform.position, posicionAsomado.position) < 0.01f)
            {
                personajeApareciendo = false;
                Invoke(nameof(IniciarEscondido), tiempoVisible);
            }
        }

        if (personajeDesapareciendo && personaje != null)
        {
            personaje.transform.position = Vector3.MoveTowards(personaje.transform.position, posicionEscondido.position, velocidadPersonaje * Time.deltaTime);

            if (Vector3.Distance(personaje.transform.position, posicionEscondido.position) < 0.01f)
            {
                personaje.enabled = false;
                personajeDesapareciendo = false;
                moverTapa = true;

                if (gameManager.instance != null)
                {
                    gameManager.instance.personajeYaAparecio = true;
                }
            }
        }

        if (moverTapa && tapa != null)
        {
            tapa.position = Vector3.MoveTowards(tapa.position, posicionFinalTapa.position, velocidadTapa * Time.deltaTime);
        }
    }

    void IniciarEscondido()
    {
        personajeDesapareciendo = true;
    }
}