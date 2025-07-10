using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PuntoAnimado
{
    public Transform puntoDestino;
    public int valorAnimacion;
}

public class SecuenciaDejaBolita : MonoBehaviour
{
    public PuntoAnimado[] puntos;             // Asignar en el inspector
    public float velocidad = 2f;
    public float distanciaMinima = 0.05f;
    public float tiempoDeEspera = 1f;
    public float tiempoInicialDeEspera = 2f;  // Tiempo antes de comenzar el ciclo
    public string nombreParametro = "estado"; // Nombre del parámetro int en el Animator

    private Animator animator;
    private int indiceActual = 0;
    public int EstadoActual => animator != null ? animator.GetInteger(nombreParametro) : -1;

    private SpriteRenderer hijoRenderer;
    private int ultimoEstado = -1;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (puntos.Length > 0)
        {
            StartCoroutine(IniciarConEspera());
        }
        hijoRenderer = GetComponentInChildren<SpriteRenderer>(true);
        if (hijoRenderer == GetComponent<SpriteRenderer>())
        {
            // Si el encontrado es el del padre, buscamos explícitamente en los hijos
            foreach (Transform child in transform)
            {
                var sr = child.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    hijoRenderer = sr;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (animator == null) return;

        int estadoActual = animator.GetInteger(nombreParametro);

        if (estadoActual != ultimoEstado)
        {
            if (estadoActual == 3 && hijoRenderer != null)
                hijoRenderer.enabled = false;
            else if (estadoActual == 0 && hijoRenderer != null)
                hijoRenderer.enabled = true;

            ultimoEstado = estadoActual;
        }
    }

    IEnumerator IniciarConEspera()
    {
        yield return new WaitForSeconds(tiempoInicialDeEspera);
        StartCoroutine(MoverEntrePuntos());
    }


    IEnumerator MoverEntrePuntos()
    {
        while (true)
        {
            PuntoAnimado actual = puntos[indiceActual];
            Transform destino = actual.puntoDestino;

            // Movimiento hacia el punto
            while (Vector3.Distance(transform.position, destino.position) > distanciaMinima)
            {
                transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);
                yield return null;
            }

            // Cambiar parámetro en el Animator
            if (animator != null)
            {
                animator.SetInteger(nombreParametro, actual.valorAnimacion);
            }

            yield return new WaitForSeconds(tiempoDeEspera);

            // Avanzar al siguiente punto
            indiceActual = (indiceActual + 1) % puntos.Length;
        }
    }

}
