using UnityEngine;
using System.Collections;

public class ArrastreNuevaBolita : MonoBehaviour
{
    public Transform objetivo; // bolita vieja o posicion de destino
    public float distanciaAceptada = 1f;
    public float fuerzaRechazo = 2f;
    public int intentosMaximos = 3;

    private Vector3 posicionInicial;
    private int intentos = 0;
    private bool arrastrando = false;
    public bool aceptada = false;
    private Rigidbody2D rb;

    public GameObject bolitaVieja; // arrastrá el objeto viejo
    public movimientoBolita movimientoScript; // arrastrá el script en la bolita nueva
    public Transform posicionInicio; // mismo que usaba la bolita vieja 
    public Transform posicionArranque; // asignar la posición de arranque de la bolita vieja
    public float velocidadMovimiento = 3f; // para controlar la velocidad del movimiento
    public bool bolitaNuevaActivada = false;

    void Start()
    {
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (arrastrando && !aceptada)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            transform.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0) && arrastrando && !aceptada)
        {
            arrastrando = false;

            float distancia = Vector3.Distance(transform.position, objetivo.position);
            if (distancia < distanciaAceptada)
            {
                intentos++;

                if (intentos < intentosMaximos)
                {
                    // Rechazo físico gradual
                    float fuerzaActual = fuerzaRechazo * ((float)(intentosMaximos - intentos) / intentosMaximos);
                    Vector2 direccion = (transform.position - objetivo.position).normalized;
                    rb.linearVelocity = direccion * fuerzaActual;

                    Invoke(nameof(ResetearPosicion), 0.5f);
                }
                else
                {
                    aceptada = true;
                    bolitaNuevaActivada = true; // 👉 activamos el flag

                    // Desactivar física
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    rb.linearVelocity = Vector2.zero;

                    // Desactivar la bolita vieja
                    if (bolitaVieja != null)
                        bolitaVieja.SetActive(false);

                    // Posicionar esta bolita en el inicio del giro
                    transform.position = posicionArranque.position;

                    StartCoroutine(MoverYActivarGiro());

                    // Activar el movimiento circular
                    if (movimientoScript != null)
                    {
                        movimientoScript.enabled = true;
                        movimientoScript.IniciarDesdeGiro();
                    }
                }
            }
        }
    }

    void OnMouseDown()
    {
        if (!aceptada)
        {
            arrastrando = true;
            rb.linearVelocity = Vector2.zero; // Detenemos cualquier impulso anterior
        }
    }

    void ResetearPosicion()
    {
        transform.position = posicionInicial;
        rb.linearVelocity = Vector2.zero;
    }

    private IEnumerator MoverYActivarGiro()
    {
        // Mover hacia posicionInicio
        while (Vector3.Distance(transform.position, posicionInicio.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionInicio.position, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }

        // Una vez llegó, activar movimientoBolita
        if (movimientoScript != null)
        {
            movimientoScript.enabled = true;
            movimientoScript.IniciarDesdeGiro();
        }
    }
}
