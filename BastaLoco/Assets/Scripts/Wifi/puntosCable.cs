using UnityEngine;

public class puntosCable : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private bool arrastrando = false;
    //public BoxCollider2D zonaValida;

    //public Rect zonaValida = new Rect(-5, -3, 10, 6); // modificar según tu escena
    //private CablePadre cablePadre;

    void Start()
    {
        cam = Camera.main;
        //cablePadre = FindFirstObjectByType<CablePadre>();
        //zonaValida = FindFirstObjectByType<BoxCollider2D>();
    }

    void OnMouseDown()
    {
        arrastrando = true;
        offset = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
{
    arrastrando = false;

    // Validar si está dentro de la zona
    /* if (zonaValida != null && !zonaValida.bounds.Contains(transform.position))
    {
        Destroy(gameObject);
    } */
}

    void Update()
    {
        if (arrastrando)
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition) + offset;
            pos.z = 0;
            transform.position = pos;
        }
    } 
}
