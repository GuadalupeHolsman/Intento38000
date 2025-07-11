using UnityEngine;

public class movimientoPlataforma : MonoBehaviour
{
    public Vector3 posicionFinal;           // Escribís la posición (x, y, z) en el Inspector
    public float velocidad = 2f;
    public DetectorConexion detector;        // Arrastrar objeto con el script "DetectorConexion"
    public string nombreAnimacionMal = "mal"; // Nombre de la animación

    private Animator animator;
    private Vector3 posicionInicial;
    private bool llego = false;
    private bool reacciono = false;
    void Start()
    {
        posicionInicial = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento hacia la posición final
        if (!llego)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionFinal, velocidad * Time.deltaTime);
            if (Vector3.Distance(transform.position, posicionFinal) < 0.01f)
            {
                llego = true;
            }
        }

        // Reaccionar si se conectó mal (pero solo una vez)
        if (llego && detector != null && detector.conectado && !reacciono)
        {
            float distancia = Vector2.Distance(detector.transform.position, detector.posicionCorrecta.position);

            if (distancia >= 0.1f && distancia < detector.tolerancia)
            {
                animator.Play(nombreAnimacionMal);
            }

            reacciono = true;
        }
    }
}
