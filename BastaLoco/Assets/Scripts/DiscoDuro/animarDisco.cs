using UnityEngine;

public class animarDisco : MonoBehaviour
{
    public ArrastreNuevaBolita arrastreBolita;
    public string parametroBool = "activar";
    public SpriteRenderer spriteOriginal; // el que está activo al principio
    public SpriteRenderer spriteNuevo;    // el que se activa cuando bolitaNuevaActivada es true
    private bool spriteYaCambiado = false;

    private Animator animator;
    private bool yaActivado = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("Animator encontrado: " + (animator != null));
    }

    void Update()
    {
        if (arrastreBolita != null && animator != null)
        {
            if (arrastreBolita.bolitaNuevaActivada && !yaActivado)
            {
                Debug.Log("Activando animación");
                animator.SetBool(parametroBool, true);
                yaActivado = true;

                // CORREGIDO: usar arrastreBolita.bolitaNuevaActivada
                if (!spriteYaCambiado)
                {
                    if (spriteOriginal != null) spriteOriginal.enabled = false;
                    if (spriteNuevo != null) spriteNuevo.enabled = true;

                    spriteYaCambiado = true;
                }
            }
        }
    }
}
