using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PuntoAnimado
{
    public Transform punto;          // El lugar donde debe ir
    public string parametro;         // Nombre del parámetro en el Animator
    public bool esTrigger;           // Si es true => SetTrigger, si es false => SetFloat
    public float valorFloat = 0f;    // Valor que se asigna si es float
}

public class SecuenciaDejaBolita : MonoBehaviour
{
    public List<PuntoAnimado> recorrido;
    public float velocidad = 2f;
    public float distanciaMinima = 0.05f;
    public float esperaFinal = 2f;

    private int indiceActual = 0;
    private Animator animator;
    private bool esperando = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        ActivarAnimacionActual();
    }

    void Update()
    {
        if (recorrido == null || recorrido.Count == 0 || esperando) return;

        Transform destino = recorrido[indiceActual].punto;
        transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, destino.position) < distanciaMinima)
        {
            indiceActual++;

            if (indiceActual >= recorrido.Count)
            {
                StartCoroutine(ReiniciarRuta());
            }
            else
            {
                ActivarAnimacionActual();
            }
        }
    }

    void ActivarAnimacionActual()
    {
        if (indiceActual >= recorrido.Count || animator == null) return;

        PuntoAnimado actual = recorrido[indiceActual];

        if (actual.esTrigger)
        {
            animator.SetTrigger(actual.parametro);
        }
        else
        {
            animator.SetFloat(actual.parametro, actual.valorFloat);
        }
    }

    IEnumerator ReiniciarRuta()
    {
        esperando = true;
        yield return new WaitForSeconds(esperaFinal);
        indiceActual = 0;
        ActivarAnimacionActual();
        esperando = false;
    }
}
