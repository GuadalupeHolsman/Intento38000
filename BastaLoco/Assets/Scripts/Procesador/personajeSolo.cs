using UnityEngine;

public class personajeSolo : MonoBehaviour
{
    public DetectorConexion detector; // Arrastrá el GameObject con el script DetectorConexion
    private Animator animator;
    private bool reacciono = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (reacciono || detector == null) return;

        if (detector.conectado)
        {
            float distancia = Vector2.Distance(detector.transform.position, detector.posicionCorrecta.position);

            if (distancia < 0.1f)
            {
                animator.SetTrigger("bien");
            }
            else
            {
                animator.SetTrigger("mal");
            }

            reacciono = true; // Para no reaccionar dos veces
        }
    }
}
