using UnityEngine;
using System.Collections;

public class caminaYDesaparece : MonoBehaviour
{
    public Vector3 puntoIntermedio1;
    public Vector3 puntoIntermedio2;  // punto en el escalón
    public Vector3 puntoFinal;        // donde deja la bolita
    private Vector3 puntoInicial;     // donde arranca (se toma en Start)

    public float velocidad = 2f;

    public GameObject bolitaHija;

    public float pausaAntesDeDejarBolita = 1f;
    public float pausaAntesDeRegresar = 1f;
    public float tiempoEsperaAntesDeReiniciar = 2f;
    public float tiempoInicialDeEspera = 2f;
    public AudioSource audioSource;
    public AudioClip sonidoBolita;

    private Animator animator;

    private enum Estado
    {
        Subiendo,
        Subiendo2,
        Caminando,
        DejandoBolita,
        Esperando,
        Volviendo1,
        Volviendo2,
        Bajando,
        EsperandoReinicio
    }

    private Estado estadoActual = Estado.Subiendo;

    void Start()
    {
        puntoInicial = transform.position;

        // Ahora la bolita empieza visible
        if (bolitaHija != null)
            bolitaHija.SetActive(true);

        animator = GetComponent<Animator>();
        StartCoroutine(EsperarInicio());
    }


    void Update()
    {
        switch (estadoActual)
        {
            case Estado.Subiendo:
                animator.Play("ram_baja2");
                MoverHacia(puntoIntermedio2, Estado.Subiendo2);
                break;

            case Estado.Subiendo2:
                animator.Play("ram_baja3");
                MoverHacia(puntoIntermedio1, Estado.Caminando);
                break;

            case Estado.Caminando:
                animator.Play("ram_camina");
                MoverHacia(puntoFinal, Estado.DejandoBolita);
                break;

            case Estado.Volviendo1:
                animator.Play("ram_regresa");
                MoverHacia(puntoIntermedio1, Estado.Volviendo2);
                break;

            case Estado.Volviendo2:
                animator.Play("ram_sube3");
                MoverHacia(puntoIntermedio2, Estado.Bajando);
                break;

            case Estado.Bajando:
                animator.Play("ram_sube2");
                MoverHacia(puntoInicial, Estado.EsperandoReinicio);
                break;

            case Estado.EsperandoReinicio:
                // no hacer nada aquí, esperar coroutine
                break;
        }
    }

    void MoverHacia(Vector3 destino, Estado siguienteEstado)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, destino) < 0.01f)
        {
            estadoActual = siguienteEstado;

            if (siguienteEstado == Estado.DejandoBolita)
            {
                StartCoroutine(ProcesoDejarBolita());
            }
            else if (siguienteEstado == Estado.EsperandoReinicio)
            {
                StartCoroutine(EsperarYReiniciar());
            }
        }
    }

    IEnumerator ProcesoDejarBolita()
    {
        animator.Play("ram_dejaBolita");

        yield return new WaitForSeconds(pausaAntesDeDejarBolita);
        
        if (audioSource != null && sonidoBolita != null)
        {
            audioSource.PlayOneShot(sonidoBolita);
        }

        if (bolitaHija != null)
            bolitaHija.SetActive(false);

        yield return new WaitForSeconds(pausaAntesDeRegresar);

        estadoActual = Estado.Volviendo1;
    }

    IEnumerator EsperarYReiniciar()
    {
        yield return new WaitForSeconds(tiempoEsperaAntesDeReiniciar);
        if (bolitaHija != null)
            bolitaHija.SetActive(true); // vuelve a aparecer al reiniciar

        estadoActual = Estado.Subiendo;
    }
    IEnumerator EsperarInicio()
    {
        yield return new WaitForSeconds(tiempoInicialDeEspera);
        estadoActual = Estado.Subiendo;
    }
}
