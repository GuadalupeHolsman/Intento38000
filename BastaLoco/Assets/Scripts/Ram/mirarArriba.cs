using UnityEngine;
using System.Collections;

public class mirarArriba : MonoBehaviour
{
    public float intervaloCambio = 3f;
    public GameObject popupVisual; // objeto con SpriteRenderer a mostrar/ocultar
    public float animacionDuracion = 0.2f; // velocidad del efecto popup

    private Animator animator;
    private float tiempo;
    private bool mirandoArriba = false;
    private Coroutine animacionPopup;

    void Start()
    {
        animator = GetComponent<Animator>();
        tiempo = intervaloCambio;

        if (popupVisual != null)
            popupVisual.SetActive(false); // oculto al inicio
    }

    void Update()
    {
        tiempo -= Time.deltaTime;

        if (tiempo <= 0)
        {
            mirandoArriba = !mirandoArriba;
            animator.SetBool("MirandoArriba", mirandoArriba);
            tiempo = intervaloCambio;

            // Mostrar u ocultar el visual
            if (popupVisual != null)
            {
                if (animacionPopup != null)
                    StopCoroutine(animacionPopup);

                animacionPopup = StartCoroutine(AnimarPopup(mirandoArriba));
            }
        }
    }

    IEnumerator AnimarPopup(bool ocultar)
    {
        if (ocultar)
        {
            // Escala hacia 0 (desaparecer)
            Vector3 escalaInicial = popupVisual.transform.localScale;
            Vector3 escalaFinal = Vector3.zero;
            float t = 0;

            while (t < animacionDuracion)
            {
                t += Time.deltaTime;
                float factor = t / animacionDuracion;
                popupVisual.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, factor);
                yield return null;
            }

            popupVisual.transform.localScale = Vector3.zero;
            popupVisual.SetActive(false);
        }
        else
        {
            popupVisual.SetActive(true);

            // Escala desde 0 a 1 (aparecer con efecto)
            popupVisual.transform.localScale = Vector3.zero;
            Vector3 escalaFinal = Vector3.one;
            float t = 0;

            while (t < animacionDuracion)
            {
                t += Time.deltaTime;
                float factor = t / animacionDuracion;
                popupVisual.transform.localScale = Vector3.Lerp(Vector3.zero, escalaFinal, factor);
                yield return null;
            }

            popupVisual.transform.localScale = escalaFinal;
        }
    }
}
