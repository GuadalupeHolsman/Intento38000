using UnityEngine;
using System.Collections;

public class caminaYDesaparece : MonoBehaviour
{
    public Vector3 puntoInicial;
    public Vector3 puntoFinal;
    public float velocidad = 2f;

    Animator animator;

    public float pausaAntesDeDesaparecer = 1f;
    public float pausaEntreHijoYPadre = 1f;
    public float pausaAntesDeVolver = 1f;

    private SpriteRenderer[] sprites;
    private SpriteRenderer spritePadre;
    private bool enMovimiento = true;

    void Start()
    {
        puntoInicial = transform.position;

        // Obtener todos los SpriteRenderer
        sprites = GetComponentsInChildren<SpriteRenderer>();
        spritePadre = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (enMovimiento)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoFinal, velocidad * Time.deltaTime);

            if (Vector3.Distance(transform.position, puntoFinal) < 0.01f)
            {
                enMovimiento = false;
                animator.enabled = false;
                StartCoroutine(EsperarYDesaparecer());
            }
        } 
    }

    IEnumerator EsperarYDesaparecer()
    {
        // Espera al llegar
        yield return new WaitForSeconds(pausaAntesDeDesaparecer);
        

        // Desaparece los hijos (todos excepto el padre)
        foreach (var sr in sprites)
        {
            if (sr != spritePadre)
                sr.enabled = false;
        }

        // Espera antes de ocultar el padre
        yield return new WaitForSeconds(pausaEntreHijoYPadre);

        if (spritePadre != null)
            spritePadre.enabled = false;

        // Espera antes de reiniciar
        yield return new WaitForSeconds(pausaAntesDeVolver);

        // Reset: posición y visibilidad
        transform.position = puntoInicial;
        enMovimiento = true;
        animator.enabled = true;

        foreach (var sr in sprites)
            sr.enabled = true;
        
    }
}
