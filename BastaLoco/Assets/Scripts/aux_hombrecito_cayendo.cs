using UnityEngine;
using System.Collections;

public class aux_hombrecito_cayendo : MonoBehaviour
{
    public Transform puntoCaida;
    public Transform puntoSalida;
    public float velocidadCaida = 2f;
    public float velocidadCaminar = 2f;
    public float tiempoTirado = 1.5f;

    public Sprite spriteTirado;
    public string nombreAnimacionCaminar = "aux_lastimado"; // nombre exacto del clip

    private enum Estado { Esperando, Cayendo, Tirado, Caminando }
    private Estado estadoActual = Estado.Esperando;

    private Animator animator;
    private SpriteRenderer sr;
    private Transform conectorPadre;
    private Vector3 ultimaPosConector;
    private bool yaSeSolto = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        conectorPadre = transform.parent;
        ultimaPosConector = conectorPadre.position;
    }

    void Update()
    {
        if (!yaSeSolto && Vector3.Distance(conectorPadre.position, ultimaPosConector) > 0.01f)
        {
            transform.parent = null;
            estadoActual = Estado.Cayendo;
            animator.Play("aux_cayendo"); // reproducí la animación directamente si querés
            yaSeSolto = true;
        }

        ultimaPosConector = conectorPadre.position;

        switch (estadoActual)
        {
            case Estado.Cayendo:
                transform.position = Vector3.MoveTowards(transform.position, puntoCaida.position, velocidadCaida * Time.deltaTime);
                if (Vector3.Distance(transform.position, puntoCaida.position) < 0.01f)
                {
                    estadoActual = Estado.Tirado;
                    animator.enabled = false;
                    sr.sprite = spriteTirado;
                    StartCoroutine(EsperarYCaminar());
                }
                break;

            case Estado.Caminando:
                transform.position = Vector3.MoveTowards(transform.position, puntoSalida.position, velocidadCaminar * Time.deltaTime);
                break;
        }
    }

    IEnumerator EsperarYCaminar()
    {
        yield return new WaitForSeconds(tiempoTirado);

        estadoActual = Estado.Caminando;
        animator.enabled = true;
        animator.Play(nombreAnimacionCaminar); // reproducimos la animación directamente
    }
}
