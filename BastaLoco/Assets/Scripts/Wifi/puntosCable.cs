using UnityEngine;

public class puntosCable : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private bool arrastrando = false;
    private PolygonCollider2D zonaValida;
    private CablePadre cablePadre;

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
    }

    void OnMouseUp()
{
    arrastrando = false;

    // Validamos si el punto está dentro del collider de la zona válida
        if (zonaValida != null && !zonaValida.OverlapPoint(transform.position))
        {
            // Notificamos al cable padre
            if (cablePadre != null)
                cablePadre.EliminarPunto(transform);

            Destroy(gameObject); // eliminamos el punto
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
    } 
}
