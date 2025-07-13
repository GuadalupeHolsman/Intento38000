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
    private bool escenaCompletada = false;

    public GameObject bolitaVieja; // arrastrá el objeto viejo
    public movimientoBolita movimientoScript; // arrastrá el script en la bolita nueva
    public Transform posicionInicio; // mismo que usaba la bolita vieja 
    public Transform posicionArranque; // asignar la posición de arranque de la bolita vieja
    public float velocidadMovimiento = 3f; // para controlar la velocidad del movimiento
    public bool bolitaNuevaActivada = false;
    public SpriteRenderer selectorObjetivo; // 👉 aparece solo cuando se arrastra la bolita
    public GameObject selectorBolita; // 👉 se desactiva cuando se acepta la bolita

    [Header("Animación de rechazo")]
    public Animator animatorRechazo; // arrastrar objeto con animator
    public Animator animatorRechazo2;
    public string triggerRechazo = "rechazo";
    public string triggerEspera = "espera";

    [Header("Vibración hacia el objetivo")]
    public float fuerzaAtraccion = 0.05f;
    public float frecuenciaVibracion = 2f;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoRechazo;
    public AudioClip sonidoAceptado;
    public AudioClip sonidoNop;

    void Start()
    {
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // MOVER MANUALMENTE CON EL MOUSE
        if (arrastrando && !aceptada)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            transform.position = mousePos;
        }

        // Mostrar el sprite solo durante el arrastre (y no si fue aceptada)
        if (selectorObjetivo != null)
        {
            selectorObjetivo.enabled = arrastrando && !aceptada;
        }

        // VIBRACIÓN CUANDO NO ESTÁ SIENDO ARRASTRADA NI ACEPTADA
        if (!arrastrando && !aceptada && objetivo != null)
        {
            Vector3 direccion = (objetivo.position - transform.position).normalized;
            float oscilacion = Mathf.Sin(Time.time * frecuenciaVibracion);
            Vector3 vibracion = direccion * fuerzaAtraccion * oscilacion;

            transform.position += vibracion;
        }

        if (Input.GetMouseButtonUp(0) && arrastrando && !aceptada)
        {
            arrastrando = false;

            float distancia = Vector3.Distance(transform.position, objetivo.position);
            if (distancia < distanciaAceptada)
            {
                intentos++;

                if (animatorRechazo != null && intentos < intentosMaximos)
                {
                    animatorRechazo.SetTrigger(triggerRechazo);
                    audioSource.PlayOneShot(sonidoNop);
                }
                if (animatorRechazo2 != null && intentos < intentosMaximos)
                {
                    animatorRechazo2.SetTrigger(triggerRechazo);
                    audioSource.PlayOneShot(sonidoNop);
                }

                if (intentos < intentosMaximos)
                {
                    // Rechazo físico gradual
                    float fuerzaActual = fuerzaRechazo * ((float)(intentosMaximos - intentos) / intentosMaximos);
                    Vector2 direccion = (transform.position - objetivo.position).normalized;
                    rb.linearVelocity = direccion * fuerzaActual;

                    Invoke(nameof(ResetearPosicion), 0.5f);

                    if (audioSource != null && sonidoRechazo != null)
                        audioSource.PlayOneShot(sonidoRechazo);
                }
                else
                {
                    aceptada = true;
                    bolitaNuevaActivada = true;

                    // DESACTIVAR EL OBJETO ASIGNADO
                    if (selectorBolita != null)
                        selectorBolita.SetActive(false);

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

                    if (audioSource != null && sonidoAceptado != null)
                        audioSource.PlayOneShot(sonidoAceptado);

                    if (!escenaCompletada && gameManager.instance != null)
                    {
                        gameManager.instance.CompletarEscena("DiscDuro", true);
                        escenaCompletada = true;
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

        // 👉 Volver a animación inicial
        if (animatorRechazo != null)
        {
            animatorRechazo.SetTrigger(triggerEspera);
        }
        if (animatorRechazo2 != null)
        {
            animatorRechazo2.SetTrigger(triggerEspera);
        }
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
