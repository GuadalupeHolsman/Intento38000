using UnityEngine;
using System.Collections;

public class seguirPunto : MonoBehaviour
{
    public Transform puntoDestino;
    public float velocidad = 2f;
    public float distanciaMinima = 0.05f;

    private bool debeMoverse = false;
    private bool esperando = false;
    private Vector3 escalaInicial;
    private Vector3 ultimoDestino;
    private static int puntosMovidos = 0;
    private static bool escenaCompletada = false;

    private Animator animator;
    private AudioSource audioSource;

    [Header("Audio")]
    public AudioClip sonidoCaminando;

    void Start()
    {
        if (puntoDestino != null)
            ultimoDestino = puntoDestino.position;

        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>(); // Asume que está en el mismo GameObject
        escalaInicial = transform.localScale;

        if (!escenaCompletada)
            puntosMovidos = 0;
    }

    void Update()
    {
        if (puntoDestino == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direccion = puntoDestino.position - transform.position;

        if (!esperando && Vector3.Distance(puntoDestino.position, ultimoDestino) > 0.01f)
        {
            StartCoroutine(EsperarAntesDeMover());
            ultimoDestino = puntoDestino.position;

            puntosMovidos++;

            if (puntosMovidos > 2 && !escenaCompletada && gameManager.instance != null)
            {
                gameManager.instance.CompletarEscena("Wifi", true);
                escenaCompletada = true;
            }
        }

        if (debeMoverse)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino.position, velocidad * Time.deltaTime);

            if (animator != null)
            {
                animator.SetFloat("dirX", direccion.normalized.x);
                animator.SetFloat("dirY", direccion.normalized.y);
            }

            if (Vector3.Distance(transform.position, puntoDestino.position) < distanciaMinima)
            {
                debeMoverse = false;

                if (animator != null)
                    animator.SetBool("caminando", false);

                // Detener sonido cuando deja de caminar
                if (audioSource != null && audioSource.isPlaying)
                    audioSource.Stop();
            }
        }

        // Escalado horizontal
        if (direccion.x > 0.01f)
            transform.localScale = escalaInicial;
        else if (direccion.x < -0.01f)
            transform.localScale = new Vector3(-escalaInicial.x, escalaInicial.y, escalaInicial.z);
    }

    IEnumerator EsperarAntesDeMover()
    {
        esperando = true;
        yield return new WaitForSeconds(0.5f);

        debeMoverse = true;
        if (animator != null) animator.SetBool("caminando", true);

        // Reproducir sonido de caminar
        if (audioSource != null && sonidoCaminando != null)
        {
            audioSource.clip = sonidoCaminando;
            audioSource.loop = true;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        esperando = false;
    }
}
