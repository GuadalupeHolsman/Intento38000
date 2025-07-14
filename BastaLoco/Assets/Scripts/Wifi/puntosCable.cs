using UnityEngine;

public class puntosCable : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private bool arrastrando = false;
    private PolygonCollider2D zonaValida;
    private CablePadre cablePadre;

    [Header("Sprites de parpadeo")]
    public SpriteRenderer contornoSprite;
    public SpriteRenderer rellenoSprite;
    public float velocidadParpadeo = 2f;

    void Start()
    {
        cam = Camera.main;
        cablePadre = FindFirstObjectByType<CablePadre>();
        if (cablePadre != null)
            zonaValida = cablePadre.zonaValida;
    }

    void OnMouseDown()
    {
        arrastrando = true;
        offset = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);

        // Ocultar sprites al arrastrar
        if (contornoSprite != null) contornoSprite.enabled = false;
        if (rellenoSprite != null) rellenoSprite.enabled = false;
    }

    void OnMouseUp()
    {
        arrastrando = false;

        // Volver a mostrar sprites cuando se suelta
        if (contornoSprite != null) contornoSprite.enabled = true;
        if (rellenoSprite != null) rellenoSprite.enabled = true;

        // Validar posición
        if (zonaValida != null && !zonaValida.OverlapPoint(transform.position))
        {
            if (cablePadre != null)
                cablePadre.EliminarPunto(transform);

            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (arrastrando)
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition) + offset;
            pos.z = 0f;
            transform.position = pos;
        }

        // Parpadeo del relleno si NO se está arrastrando
        if (!arrastrando && rellenoSprite != null)
        {
            Color c = rellenoSprite.color;
            c.a = Mathf.PingPong(Time.time * velocidadParpadeo, 1f);
            rellenoSprite.color = c;
        }
    }
}
