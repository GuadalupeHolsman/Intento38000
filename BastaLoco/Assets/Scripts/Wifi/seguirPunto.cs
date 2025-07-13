using UnityEngine;
using System.Collections;

public class seguirPunto : MonoBehaviour
{
    public Transform puntoDestino;
    public float velocidad = 2f;
    public float distanciaMinima = 0.05f;

    private bool debeMoverse = false;
    private bool esperando = false;
    private Vector3 escalaInicial;
    private Vector3 ultimoDestino;
    private static int puntosMovidos = 0;
    private static bool escenaCompletada = false;

    private Animator animator;

    void Start()
    {
        if (puntoDestino != null)
            ultimoDestino = puntoDestino.position;

        animator = GetComponentInChildren<Animator>();
        escalaInicial = transform.localScale;

        if (!escenaCompletada)
            puntosMovidos = 0;
    }

    void Update()
    {
        //Los personajes caen
        /* if (puntoDestino == null)
        {
            if (animator != null)
            {
                animator.SetBool("caminando", false);
                animator.SetBool("cayendo", true); // parámetro opcional si tenés animación de caída
            }

            // Caída hacia abajo si pierde el punto
            transform.position += Vector3.down * velocidad * Time.deltaTime;
            return;
        }  */

        //Los personajes desaparecen
        if (puntoDestino == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direccion = puntoDestino.position - transform.position;

        // Solo calcular si hay movimiento
        if (!esperando && Vector3.Distance(puntoDestino.position, ultimoDestino) > 0.01f)
        {
            StartCoroutine(EsperarAntesDeMover());
            ultimoDestino = puntoDestino.position;

            puntosMovidos++;

            // Cuando se mueven más de 2 puntos, marcamos la escena como completada
            if (puntosMovidos > 2 && !escenaCompletada && gameManager.instance != null)
            {
                gameManager.instance.CompletarEscena("Wifi", true);
                escenaCompletada = true;
            }
        }

        if (debeMoverse)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino.position, velocidad * Time.deltaTime);

            // Actualizar animación de dirección
            if (animator != null)
            {
                animator.SetFloat("dirX", direccion.normalized.x);
                animator.SetFloat("dirY", direccion.normalized.y);
            }

            if (Vector3.Distance(transform.position, puntoDestino.position) < distanciaMinima)
            {
                debeMoverse = false;
                if (animator != null) animator.SetBool("caminando", false);
            }
        }

        // Escalado horizontal (solo para rotar sprite si es necesario)
        if (direccion.x > 0.01f)
            transform.localScale = escalaInicial;
        else if (direccion.x < -0.01f)
            transform.localScale = new Vector3(-escalaInicial.x, escalaInicial.y, escalaInicial.z);
    }

    IEnumerator EsperarAntesDeMover()
    {
        esperando = true;
        yield return new WaitForSeconds(0.5f);

        debeMoverse = true;
        if (animator != null) animator.SetBool("caminando", true);
        esperando = false;
    }
}
