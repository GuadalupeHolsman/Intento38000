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

        Vector3 direccion = puntoDestino.position - transform.position;

        // Solo calcular si hay movimiento
        if (!esperando && Vector3.Distance(puntoDestino.position, ultimoDestino) > 0.01f)
        {
            StartCoroutine(EsperarAntesDeMover());
            ultimoDestino = puntoDestino.position;
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
        yield return new WaitForSeconds(1f);

        debeMoverse = true;
        if (animator != null) animator.SetBool("caminando", true);
        esperando = false;
    }
}
