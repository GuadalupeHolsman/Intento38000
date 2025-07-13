using UnityEngine;

public class OcultarZonaEscenaCompletada : MonoBehaviour
{
    public string nombreEscena;

    [Header("Sprites a usar")]
    public Sprite spriteBorde;
    public Sprite spriteRelleno;

    [Header("Ajustes de parpadeo")]
    public float velocidadParpadeo = 2f;
    public float alphaMin = 0.2f;
    public float alphaMax = 1f;

    private SpriteRenderer srBorde;
    private SpriteRenderer srRelleno;
    private bool parpadeoActivo = true;

    void Awake()
    {
        // Crear objeto para el borde
        GameObject objBorde = new GameObject("SpriteBorde");
        objBorde.transform.SetParent(transform);
        objBorde.transform.localPosition = Vector3.zero;
        srBorde = objBorde.AddComponent<SpriteRenderer>();
        srBorde.sprite = spriteBorde;
        srBorde.sortingOrder = 2;
        srBorde.color = new Color(1, 1, 1, 1);

        // Crear objeto para el relleno
        GameObject objRelleno = new GameObject("SpriteRelleno");
        objRelleno.transform.SetParent(transform);
        objRelleno.transform.localPosition = Vector3.zero;
        srRelleno = objRelleno.AddComponent<SpriteRenderer>();
        srRelleno.sprite = spriteRelleno;
        srRelleno.sortingOrder = 1; // Relleno detrás del borde
        srRelleno.color = new Color(1, 1, 1, 1);
    }

    void Start()
    {
        if (gameManager.instance == null)
        {
            enabled = false;
            return;
        }

        if (gameManager.instance.progreso.TryGetValue(nombreEscena, out bool completada) && completada)
        {
            srBorde.enabled = false;
            srRelleno.enabled = false;
            parpadeoActivo = false;
        }
    }

    void Update()
    {
        if (!parpadeoActivo) return;

        float t = (Mathf.Sin(Time.time * velocidadParpadeo) + 1f) / 2f;

        float alphaBorde = Mathf.Lerp(alphaMin, alphaMax, t);
        float alphaRelleno = Mathf.Lerp(alphaMax, alphaMin, t); // Inverso

        srBorde.color = new Color(1f, 1f, 1f, alphaBorde);
        srRelleno.color = new Color(1f, 1f, 1f, alphaRelleno);
    }

}

