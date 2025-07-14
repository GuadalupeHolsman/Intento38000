using UnityEngine;
using System.Collections;

public class mirarArriba : MonoBehaviour
{
    public float intervaloCambio = 3f;
    public GameObject popupVisual;
    public float animacionDuracion = 0.2f;
    public Vector3 escalaFinalDeseada = new Vector3(4f, 4f, 1f);

    private Animator animator;
    private float tiempo;
    private bool mirandoArriba = false;
    private Coroutine animacionPopup;
    public float delayInicialPopup = 5f;
    private bool popupHabilitado = false;

    private Vector3 escalaOriginal;

    /* public AudioSource audioSource;
    public AudioClip sonidoPopup; */

    void Start()
    {
        animator = GetComponent<Animator>();
        tiempo = intervaloCambio;

        if (popupVisual != null)
        {
            popupVisual.SetActive(false);
            popupVisual.transform.localScale = Vector3.zero;
        }

        // Esperar x segundos antes de habilitar popup
        StartCoroutine(HabilitarPopupConRetraso());
    }

    IEnumerator HabilitarPopupConRetraso()
    {
        yield return new WaitForSeconds(delayInicialPopup);
        popupHabilitado = true;
    }

    void Update()
    {
        tiempo -= Time.deltaTime;

        if (tiempo <= 0)
        {
            mirandoArriba = !mirandoArriba;
            animator.SetBool("MirandoArriba", mirandoArriba);
            tiempo = intervaloCambio;

            if (popupVisual != null && popupHabilitado)
            {
                if (animacionPopup != null)
                    StopCoroutine(animacionPopup);

                animacionPopup = StartCoroutine(AnimarPopup(mirandoArriba));
            }
        }
    }

    IEnumerator AnimarPopup(bool mostrar)
    {
        if (mostrar)
        {
            popupVisual.SetActive(true);
            popupVisual.transform.localScale = Vector3.zero;

            float t = 0;
            while (t < animacionDuracion)
            {
                t += Time.deltaTime;
                float factor = t / animacionDuracion;
                popupVisual.transform.localScale = Vector3.Lerp(Vector3.zero, escalaFinalDeseada, factor);

                /* if (audioSource != null && sonidoPopup != null)
                {
                    audioSource.PlayOneShot(sonidoPopup);
                } */

                yield return null;
            }

            popupVisual.transform.localScale = escalaFinalDeseada;
        }
        else
        {
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

            popupVisual.transform.localScale = escalaFinal;
            popupVisual.SetActive(false);

            /* if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            } */
        }
    }
}
