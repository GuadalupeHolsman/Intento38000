using UnityEngine;
using System.Collections;

public class aux_hombrecito_cayendo : MonoBehaviour
{
    public Transform puntoCaida;
    public Transform puntoSalida;
    public float velocidadCaida = 2f;
    public float velocidadCaminar = 2f;
    public float tiempoTirado = 1.5f;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoEsperando;
    public AudioClip sonidoTirado;
    public AudioClip sonidoCaminando;

    private Animator animator;
    private Transform conectorPadre;
    private Vector3 ultimaPosConector;

    private enum Estado { Esperando, Cayendo, Tirado, Caminando }
    private Estado estadoActual = Estado.Esperando;

    void Start()
    {
        animator = GetComponent<Animator>();
        conectorPadre = transform.parent;
        ultimaPosConector = conectorPadre.position;

        // Opcional: sonido inicial
        if (sonidoEsperando != null && audioSource != null)
            audioSource.PlayOneShot(sonidoEsperando);
    }

    void Update()
    {
        switch (estadoActual)
        {
            case Estado.Esperando:
                if (Vector3.Distance(conectorPadre.position, ultimaPosConector) > 0.01f)
                {
                    transform.parent = null;
                    estadoActual = Estado.Cayendo;
                    animator.SetBool("cayendo", true);
                }
                break;

            case Estado.Cayendo:
                transform.position = Vector3.MoveTowards(transform.position, puntoCaida.position, velocidadCaida * Time.deltaTime);
                if (Vector3.Distance(transform.position, puntoCaida.position) < 0.01f)
                {
                    estadoActual = Estado.Tirado;
                    animator.SetBool("cayendo", false);
                    animator.SetBool("tirado", true);

                    if (sonidoTirado != null && audioSource != null)
                        audioSource.PlayOneShot(sonidoTirado);

                    StartCoroutine(EsperarYCaminar());
                }
                break;

            case Estado.Caminando:
                transform.position = Vector3.MoveTowards(transform.position, puntoSalida.position, velocidadCaminar * Time.deltaTime);
                break;
        }

        ultimaPosConector = conectorPadre.position;
    }

    IEnumerator EsperarYCaminar()
    {
        yield return new WaitForSeconds(tiempoTirado);
        animator.SetBool("tirado", false);
        animator.SetBool("caminando", true);
        estadoActual = Estado.Caminando;

        if (sonidoCaminando != null && audioSource != null)
            audioSource.PlayOneShot(sonidoCaminando);
    }
}
