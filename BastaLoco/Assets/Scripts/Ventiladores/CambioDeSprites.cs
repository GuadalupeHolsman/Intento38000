using UnityEngine;

public class CambioDeSprites : MonoBehaviour
{
    private Animator animator;
    private BalanzaManager balanza;

    [Tooltip("Nombre del parámetro Float en el Animator")]
    public string nombreParametro = "temperatura";

    void Start()
    {
        animator = GetComponent<Animator>();
        balanza = FindAnyObjectByType<BalanzaManager>();

        if (animator == null)
            Debug.LogError($"No se encontró Animator en {gameObject.name}");
        if (balanza == null)
            Debug.LogError("No se encontró BalanzaManager en escena");
    }

    void Update()
    {
        if (animator != null && balanza != null)
        {
            // Pasamos la temperatura como parámetro float al Animator
            animator.SetFloat(nombreParametro, balanza.temperatura);
        }
    }
}
