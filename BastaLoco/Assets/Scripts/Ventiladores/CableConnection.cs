using UnityEngine;

public class CableConnection : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject ventiladorFrio;
    public GameObject ventiladorCalor;
    public ParticleSystem particulasFrio;
    public ParticleSystem particulasCalor;
    public GameObject indicador;
    public BalanzaManager balanzaManager;

    [Header("Sprite de selección inicial")]
    public GameObject spriteSeleccion;

    [Header("Animación del spriteSeleccion")]
    public Sprite spriteAnimacion1;
    public Sprite spriteAnimacion2;
    public float alphaMin = 0.3f;
    public float alphaMax = 1f;
    public float velocidadFade = 2f;
    private SpriteRenderer spriteSeleccionRenderer;
    private float alphaActual = 1f;
    private bool bajandoAlpha = true;
    private bool usandoSprite1 = true;

    [Header("Sonido")]
    public AudioClip sonidoConexion;
    private AudioSource audioSource;

    [Header("Velocidad de cambio")]
    public float tiempoCambio = 1f;

    [Header("Sprites del cable")]
    public Sprite spriteDesconectado;
    public Sprite spriteConectado;
    public SpriteRenderer spriteRenderer;

    private GameObject ventiladorConectado;
    private float tiempoActual = 0f;

    private bool estaSiendoAgarrado = false;
    private float tiempoInactivo = 0f;
    private float tiempoParaMostrarIndicador = 5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (spriteRenderer != null && spriteDesconectado != null)
            spriteRenderer.sprite = spriteDesconectado;

        if (spriteSeleccion != null)
        {
            spriteSeleccion.SetActive(true);
            spriteSeleccionRenderer = spriteSeleccion.GetComponent<SpriteRenderer>();
            alphaActual = alphaMax;

            if (spriteAnimacion1 != null)
                spriteSeleccionRenderer.sprite = spriteAnimacion1;
        }
    }

    void OnMouseDown()
    {
        estaSiendoAgarrado = true;

        if (spriteSeleccion != null)
            spriteSeleccion.SetActive(false);

        tiempoInactivo = 0f;
    }

    void OnMouseUp()
    {
        estaSiendoAgarrado = false;
        tiempoInactivo = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == ventiladorFrio || other.gameObject == ventiladorCalor)
        {
            ventiladorConectado = other.gameObject;
            tiempoActual = 0f;

            if (spriteRenderer != null && spriteConectado != null)
                spriteRenderer.sprite = spriteConectado;

            if (spriteSeleccion != null)
                spriteSeleccion.SetActive(false);

            if (ventiladorConectado == ventiladorFrio && particulasFrio != null)
                particulasFrio.Play();
            else if (ventiladorConectado == ventiladorCalor && particulasCalor != null)
                particulasCalor.Play();

            if (sonidoConexion != null && audioSource != null)
                audioSource.PlayOneShot(sonidoConexion);

            Debug.Log("🔌 Cable conectado a: " + other.gameObject.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == ventiladorConectado)
        {
            if (spriteRenderer != null && spriteDesconectado != null)
                spriteRenderer.sprite = spriteDesconectado;

            if (ventiladorConectado == ventiladorFrio && particulasFrio != null)
                particulasFrio.Stop();
            else if (ventiladorConectado == ventiladorCalor && particulasCalor != null)
                particulasCalor.Stop();

            ventiladorConectado = null;
            tiempoInactivo = 0f;

            Debug.Log("⚡ Cable desconectado de: " + other.gameObject.name);
        }
    }

    void Update()
    {
        if (ventiladorConectado != null)
        {
            tiempoActual += Time.deltaTime;

            if (tiempoActual >= tiempoCambio)
            {
                if (ventiladorConectado == ventiladorFrio)
                    balanzaManager.ReducirTemperatura();
                else if (ventiladorConectado == ventiladorCalor)
                    balanzaManager.AumentarTemperatura();

                tiempoActual = 0f;
            }
        }

        // Mostrar el sprite después de 5 segundos sin uso
        if (ventiladorConectado == null && !estaSiendoAgarrado)
        {
            tiempoInactivo += Time.deltaTime;

            if (tiempoInactivo >= tiempoParaMostrarIndicador)
            {
                if (spriteSeleccion != null && !spriteSeleccion.activeSelf)
                    spriteSeleccion.SetActive(true);
            }
        }
        else
        {
            tiempoInactivo = 0f;
        }

        // 🌟 Animación suave + cambio de sprite
        if (spriteSeleccion != null && spriteSeleccion.activeSelf && spriteSeleccionRenderer != null)
        {
            float deltaAlpha = velocidadFade * Time.deltaTime;

            if (bajandoAlpha)
            {
                alphaActual -= deltaAlpha;
                if (alphaActual <= alphaMin)
                {
                    alphaActual = alphaMin;
                    bajandoAlpha = false;

                    // Cambiar sprite
                    usandoSprite1 = !usandoSprite1;
                    spriteSeleccionRenderer.sprite = usandoSprite1 ? spriteAnimacion1 : spriteAnimacion2;
                }
            }
            else
            {
                alphaActual += deltaAlpha;
                if (alphaActual >= alphaMax)
                {
                    alphaActual = alphaMax;
                    bajandoAlpha = true;

                    // Cambiar sprite
                    usandoSprite1 = !usandoSprite1;
                    spriteSeleccionRenderer.sprite = usandoSprite1 ? spriteAnimacion1 : spriteAnimacion2;
                }
            }

            Color colorActual = spriteSeleccionRenderer.color;
            spriteSeleccionRenderer.color = new Color(colorActual.r, colorActual.g, colorActual.b, alphaActual);
        }
    }
}