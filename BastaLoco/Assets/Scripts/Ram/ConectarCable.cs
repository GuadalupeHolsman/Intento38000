using UnityEngine;

public class ArrastrarCable2D : MonoBehaviour
{
    private bool arrastrando = false;
    private Camera camara;
    private Vector3 offset;

    [Header("Conexión")]
    public Transform puntoConexion;                     // Transform del destino
    public float distanciaConexion = 0.5f;              // Rango de snap
    public bool conectado = false;

    [Header("Cambio visual al conectar")]
    public SpriteRenderer spriteDelDestino;             // SpriteRenderer del conector en el destino
    public Sprite spriteConectado;                      // Sprite a mostrar cuando se conecta

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

                // Cambio de sprite visual
                if (spriteDelDestino != null && spriteConectado != null)
                {
                    spriteDelDestino.sprite = spriteConectado;
                }

                Debug.Log("¡Conectado!");
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
    }
}
