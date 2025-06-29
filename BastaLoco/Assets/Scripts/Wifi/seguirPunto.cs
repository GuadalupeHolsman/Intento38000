using UnityEngine;
using System.Collections;

public class seguirPunto : MonoBehaviour
{
    public Transform puntoDestino;         // Referencia al punto que debe seguir
    public float velocidad = 2f;           // Velocidad de movimiento
    public float distanciaMinima = 0.05f;  // Distancia para detenerse
    private bool debeMoverse = false;
    private bool esperando = false;
    private Vector3 escalaInicial;

    private Vector3 ultimoDestino;
    private Animator animator;

    void Start()
    {
        if (puntoDestino != null)
            ultimoDestino = puntoDestino.position;

        animator = GetComponentInChildren<Animator>();
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        if (puntoDestino == null) return;

        if (!esperando && Vector3.Distance(puntoDestino.position, ultimoDestino) > 0.01f)
        {
            StartCoroutine(EsperarAntesDeMover());
            ultimoDestino = puntoDestino.position;
        }

        if (debeMoverse)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino.position, velocidad * Time.deltaTime);

            if (Vector3.Distance(transform.position, puntoDestino.position) < distanciaMinima)
            {
                debeMoverse = false;
                if (animator != null) animator.SetBool("caminando", false);
            }
        }

         Vector3 direccion = puntoDestino.position - transform.position;
        if (direccion.x > 0.01f)
            transform.localScale = escalaInicial;  // mirando derecha
        else if (direccion.x < -0.01f)
            transform.localScale = new Vector3(-escalaInicial.x, escalaInicial.y, escalaInicial.z);  // mirando izquierda
    }

    IEnumerator EsperarAntesDeMover()
    {
        esperando = true;
        yield return new WaitForSeconds(1f); // Espera 1 segundo

        debeMoverse = true;
        if (animator != null) animator.SetBool("caminando", true);
        esperando = false;
    } 
}
