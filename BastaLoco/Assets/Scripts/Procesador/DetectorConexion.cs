using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DetectorConexion : MonoBehaviour
{
    // Referencias básicas
    public Transform posicionCorrecta;
    public GameObject bolitaPrefab;
    public GameObject rayaPrefab;
    public float tolerancia = 1.2f;

    // Indicador inicial que parpadea
    public GameObject indicadorSeleccion;
    public Sprite spriteA;
    public Sprite spriteB;
    public float velocidadParpadeo = 0.5f;

    // Animaciones progresivas para feedback
    public GameObject animacionCorrectaGO;
    public Sprite[] spritesCorrecto;

    public GameObject animacionIncorrectaGO;
    public Sprite[] spritesIncorrecto;

    public float tiempoEntreFrames = 0.3f; // Más lento para que dure más
    public int repeticionesAnimacion = 3;  // Cantidad de veces que titilea la animación

    // Variables internas
    public bool conectado = false;
    private Vector3 offset;
    private bool arrastrando = false;
    private Camera camara;
    private bool bienConectado = false;
    private bool malConectado = false;
    private bool escenaCompletada = false;
    private float esperaAntesError = 4f;

    private SpriteRenderer indicadorRenderer;
    private Coroutine animacionIndicador;

    void Start()
    {
        camara = Camera.main;

        if (indicadorSeleccion != null)
        {
            indicadorSeleccion.SetActive(true);
            indicadorRenderer = indicadorSeleccion.GetComponent<SpriteRenderer>();

            if (indicadorRenderer != null)
            {
                animacionIndicador = StartCoroutine(AnimarIndicador());
            }
        }

        // Asegurar que animaciones feedback estén apagadas al inicio
        if (animacionCorrectaGO != null) animacionCorrectaGO.SetActive(false);
        if (animacionIncorrectaGO != null) animacionIncorrectaGO.SetActive(false);
    }

    void OnMouseDown()
    {
        if (conectado) return;

        offset = transform.position - camara.ScreenToWorldPoint(Input.mousePosition);
        arrastrando = true;

        if (indicadorSeleccion != null)
        {
            indicadorSeleccion.SetActive(false);

            if (animacionIndicador != null)
            {
                StopCoroutine(animacionIndicador);
            }
        }
    }

    void OnMouseDrag()
    {
        if (arrastrando && !conectado)
        {
            Vector3 mousePosition = camara.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x + offset.x, mousePosition.y + offset.y, 0f);

            float distancia = Vector2.Distance(transform.position, posicionCorrecta.position);
            Debug.Log("Distancia actual: " + distancia);

            if (distancia < 0.1f)
            {
                Debug.Log("¡Estás en la posición exacta!");
            }
            else if (distancia < tolerancia)
            {
                Debug.Log("Casi correcto, ajustá un poco más.");
            }
            else
            {
                Debug.Log("Muy lejos, acercate.");
            }
        }
    }

    void OnMouseUp()
    {
        if (conectado) return;

        arrastrando = false;

        float distancia = Vector2.Distance(transform.position, posicionCorrecta.position);
        Debug.Log("Distancia al soltar: " + distancia);

        if (distancia < 0.1f)
        {
            Debug.Log("¡Conexión correcta! Posición aceptada.");
            Instantiate(bolitaPrefab, posicionCorrecta.position, Quaternion.identity);
            transform.position = posicionCorrecta.position;
            conectado = true;
            bienConectado = true;

            if (animacionCorrectaGO != null && spritesCorrecto.Length > 0)
            {
                StartCoroutine(ReproducirAnimacion(animacionCorrectaGO, spritesCorrecto));
            }
        }
        else if (distancia < tolerancia)
        {
            Debug.Log("Conexión aceptada, pero no perfecta.");
            Instantiate(rayaPrefab, transform.position, Quaternion.identity);
            malConectado = true;
            conectado = true;

            if (animacionIncorrectaGO != null && spritesIncorrecto.Length > 0)
            {
                StartCoroutine(ReproducirAnimacion(animacionIncorrectaGO, spritesIncorrecto));
            }
        }
        else
        {
            Debug.Log("Posición incorrecta. Reintentá.");

            if (animacionIncorrectaGO != null && spritesIncorrecto.Length > 0)
            {
                StartCoroutine(ReproducirAnimacion(animacionIncorrectaGO, spritesIncorrecto));
            }
        }

        if (bienConectado && !escenaCompletada && gameManager.instance != null)
        {
            gameManager.instance.CompletarEscena("Procesador", true);
            escenaCompletada = true;
        }

        if (malConectado)
        {
            StartCoroutine(EscenaError());
        }
    }

    IEnumerator EscenaError()
    {
        yield return new WaitForSeconds(esperaAntesError);
        SceneManager.LoadScene("Error");
    }

    IEnumerator AnimarIndicador()
    {
        bool bajando = true;
        bool usandoA = true;

        while (true)
        {
            if (indicadorRenderer != null)
            {
                indicadorRenderer.sprite = usandoA ? spriteA : spriteB;
                usandoA = !usandoA;
            }

            for (float t = 0f; t < velocidadParpadeo; t += Time.deltaTime)
            {
                if (indicadorRenderer != null)
                {
                    float paso = t / velocidadParpadeo;
                    float nuevaAlpha = bajando ? Mathf.Lerp(1f, 0.3f, paso) : Mathf.Lerp(0.3f, 1f, paso);
                    Color c = indicadorRenderer.color;
                    c.a = nuevaAlpha;
                    indicadorRenderer.color = c;
                }
                yield return null;
            }

            bajando = !bajando;
        }
    }

    IEnumerator ReproducirAnimacion(GameObject contenedor, Sprite[] secuencia)
    {
        SpriteRenderer sr = contenedor.GetComponent<SpriteRenderer>();
        contenedor.SetActive(true);

        for (int ciclo = 0; ciclo < repeticionesAnimacion; ciclo++)
        {
            foreach (Sprite frame in secuencia)
            {
                sr.sprite = frame;

                // Fade in
                for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime * 5f)
                {
                    Color c = sr.color;
                    c.a = alpha;
                    sr.color = c;
                    yield return null;
                }

                // Esperar un poco con opacidad alta
                yield return new WaitForSeconds(tiempoEntreFrames);

                // Fade out (salvo en el último ciclo)
                if (ciclo < repeticionesAnimacion - 1)
                {
                    for (float alpha = 1f; alpha >= 0f; alpha -= Time.deltaTime * 5f)
                    {
                        Color c = sr.color;
                        c.a = alpha;
                        sr.color = c;
                        yield return null;
                    }
                }
            }
        }

        // En el último ciclo, dejamos la animación fija en el último frame con opacidad alta
        Color finalColor = sr.color;
        finalColor.a = 1f;
        sr.color = finalColor;

        // No hacemos fade out ni desactivamos el contenedor, queda fijo visible
    }
}