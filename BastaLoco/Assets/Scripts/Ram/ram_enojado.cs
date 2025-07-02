using UnityEngine;

public class ram_enojado : MonoBehaviour
{
    public Animator animator;
    public float tiempoParaEnojarse = 5f;
    public GameObject gestorConexionesGO;

    private gestorConexiones gestor;
    private int pasoAnterior = -1;
    private float tiempoInactivo = 0f;
    private bool enojado = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        gestor = gestorConexionesGO.GetComponent<gestorConexiones>();
        if (gestor == null)
            Debug.LogError("No se encontró el script gestorConexiones en el GameObject asignado");
    }

    void Update()
{
    if (gestor == null) return;

    // Si ya terminó todo, volver a pestañear
    if (gestor.conexionesCompletas)
    {
        animator.SetBool("Enojado", false);
        enojado = false;
        return;
    }

    int pasoActual = ObtenerPasoActual();

    // Si el paso no avanzó
    if (pasoActual == pasoAnterior)
    {
        tiempoInactivo += Time.deltaTime;

        if (tiempoInactivo >= tiempoParaEnojarse && !enojado)
        {
            animator.SetBool("Enojado", true);
            enojado = true;
        }
    }
    else
{
    // Avanzó el paso: reseteamos el tiempo
    tiempoInactivo = 0f;
    pasoAnterior = pasoActual;

    // Solo se desenoja si ya se completaron todas las conexiones
    if (gestor.conexionesCompletas && enojado)
    {
        animator.SetBool("Enojado", false);
        enojado = false;
    }
}
}


    int ObtenerPasoActual()
    {
        // Usamos reflexión para leer el campo privado sin modificar el script original
        var campo = typeof(gestorConexiones).GetField("pasoActual", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (int)campo.GetValue(gestor);
    }

}
