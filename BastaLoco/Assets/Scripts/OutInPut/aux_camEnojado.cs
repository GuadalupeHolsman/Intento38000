using UnityEngine;

public class aux_camEnojado : MonoBehaviour
{
    public Transform puntoA; // punto inicial
    public Transform puntoB; // punto destino
    public float velocidad = 2f;
    public float tiempoEspera = 1f;

    private bool yendoA = true;
    private bool esperando = false;

    private Vector3 escalaOriginal;

    void Start()
    {
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        if (esperando) return;

        Transform destino = yendoA ? puntoB : puntoA;
        transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, destino.position) < 0.01f)
        {
            StartCoroutine(EsperarYVolver());
        }
    }

    System.Collections.IEnumerator EsperarYVolver()
    {
        esperando = true;

        yield return new WaitForSeconds(tiempoEspera);

        yendoA = !yendoA;

        // Rotar horizontalmente en X
        Vector3 escala = escalaOriginal;
        escala.x *= yendoA ? 1 : -1;
        transform.localScale = escala;

        esperando = false;
    }
}
