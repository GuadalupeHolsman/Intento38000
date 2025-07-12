using UnityEngine;
using System.Collections;

public class movimientoDupla : MonoBehaviour
{
    public DetectorConexion detector;        // Referencia al script DetectorConexion
    public string animacionBien = "bien";
    public string animacionMal1 = "mal1";
    public string animacionMal2 = "mal2";
    public string animacionMal3 = "mal3";

    public Transform punto1; // Destino 1 para mal
    public Transform punto2; // Destino 2
    public Transform punto3; // Destino 3

    public float velocidad = 2f;
    public float esperaAntesDeEmpezar = 1f;

    private Animator animator;
    private bool reacciono = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!reacciono && detector != null && detector.conectado)
        {
            reacciono = true;

            float distancia = Vector2.Distance(detector.transform.position, detector.posicionCorrecta.position);

            if (distancia < 0.1f)
            {
                // Conexión buena
                animator.Play(animacionBien);
            }
            else
            {
                // Conexión mala: iniciar secuencia
                StartCoroutine(SecuenciaMal());
            }
        }
    }

    IEnumerator SecuenciaMal()
    {
        yield return new WaitForSeconds(esperaAntesDeEmpezar);

        // 1. Primera animación y movimiento
    animator.SetTrigger("anim1");
    yield return StartCoroutine(MoverHasta(punto1));

    // 2. Segunda animación y movimiento
    animator.SetTrigger("anim2");
    yield return StartCoroutine(MoverHasta(punto2));

    // 3. Tercera animación y movimiento
    animator.SetTrigger("anim3");
    yield return StartCoroutine(MoverHasta(punto3));
    }

    IEnumerator MoverHasta(Transform destino)
    {
        while (Vector3.Distance(transform.position, destino.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);
            yield return null;
        }
    }
}
