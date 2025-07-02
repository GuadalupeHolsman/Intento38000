using UnityEngine;

public class mirarArriba : MonoBehaviour
{
    public float intervaloCambio = 3f; // tiempo entre cambios

    private Animator animator;
    private float tiempo;
    private bool mirandoArriba = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        tiempo = intervaloCambio;
    }

    void Update()
    {
        tiempo -= Time.deltaTime;

        if (tiempo <= 0)
        {
            mirandoArriba = !mirandoArriba;
            animator.SetBool("MirandoArriba", mirandoArriba);
            tiempo = intervaloCambio;
        }
    }
}
