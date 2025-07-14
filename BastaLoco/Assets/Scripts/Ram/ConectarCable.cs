using UnityEngine;
using System.Collections.Generic;

public class ArrastrarCable2D : MonoBehaviour
{
    private bool arrastrando = false;
    private Camera camara;
    private Vector3 offset;
    public AudioSource audioSource;
    public AudioClip sonidoConexion;

    public UnityEngine.Events.UnityEvent alConectar;

    [Header("Conexión")]
    public Transform puntoConexion;                     // Transform del destino
    public float distanciaConexion = 0.5f;              // Rango de snap
    public bool conectado = false;

    [Header("Indicador visual")]
    public SpriteRenderer indicadorContorno;
    public SpriteRenderer indicadorRelleno;
    public float velocidadParpadeo = 2f;
    [Range(0f, 1f)] public float alphaMin = 0.2f;
    [Range(0f, 1f)] public float alphaMax = 1f;

    //[Header("Cambio visual al conectar")]
    //public SpriteRenderer spriteDelDestino;             // SpriteRenderer del conector en el destino
    //public Sprite spriteConectado;                      // Sprite a mostrar cuando se conecta

    void Start()
    {
        camara = Camera.main;
    }

    void OnMouseDown()
    {
        if (conectado) return;
        arrastrando = true;
        offset = transform.position - camara.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        arrastrando = false;

        // Verificamos si está cerca del punto de conexión
        if (puntoConexion != null)
        {
            float distancia = Vector2.Distance(transform.position, puntoConexion.position);
            if (distancia < distanciaConexion)
            {
                transform.position = puntoConexion.position;
                conectado = true;
                alConectar.Invoke();

                // Cambio de sprite visual
                /* if (spriteDelDestino != null && spriteConectado != null)
                {
                    spriteDelDestino.sprite = spriteConectado;
                } */

                //Debug.Log("¡Conectado!");

                if (audioSource != null && sonidoConexion != null)
                {
                    audioSource.PlayOneShot(sonidoConexion);
                }
            }
        }
    }

    void Update()
    {
        if (arrastrando)
        {
            Vector3 pos = camara.ScreenToWorldPoint(Input.mousePosition) + offset;
            pos.z = 0f;
            transform.position = pos;
        }
        // Animar parpadeo mientras no está conectado
        if (!conectado)
        {
            if (indicadorRelleno != null)
            {
                float t = (Mathf.Sin(Time.time * velocidadParpadeo) + 1f) / 2f;
                float alpha = Mathf.Lerp(alphaMin, alphaMax, t);

                Color c = indicadorRelleno.color;
                c.a = alpha;
                indicadorRelleno.color = c;
            }
        }
        else
        {
            // Ocultar ambos cuando se conecta
            if (indicadorContorno != null)
                indicadorContorno.enabled = false;

            if (indicadorRelleno != null)
                indicadorRelleno.enabled = false;
        }
    }
}
