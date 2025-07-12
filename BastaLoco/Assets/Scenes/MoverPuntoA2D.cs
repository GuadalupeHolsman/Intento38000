using UnityEngine;

public class MoverPuntoA2D : MonoBehaviour
{
    private Camera cam;
    private Rigidbody2D rb;

    private bool estaSiendoArrastrado = false;

    public Transform puntoB; // Referencia al punto fijo
    public float distanciaMaxima = 5f; // Máxima distancia permitida desde puntoB

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        // Detectar si el mouse empieza sobre este objeto
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(mouseWorld);

            if (col != null && col.gameObject == gameObject)
            {
                estaSiendoArrastrado = true;
            }
        }

        // Si está siendo arrastrado, moverlo con el mouse pero limitando la distancia
        if (estaSiendoArrastrado && Input.GetMouseButton(0))
        {
            Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);

            // Calcular distancia desde PuntoB
            Vector2 desdePuntoB = mouseWorld - (Vector2)puntoB.position;

            if (desdePuntoB.magnitude > distanciaMaxima)
            {
                // Limitar la posición al círculo de radio máximo
                mouseWorld = (Vector2)puntoB.position + desdePuntoB.normalized * distanciaMaxima;
            }

            rb.MovePosition(mouseWorld);
        }

        // Soltar
        if (Input.GetMouseButtonUp(0))
        {
            estaSiendoArrastrado = false;
        }
    }
}