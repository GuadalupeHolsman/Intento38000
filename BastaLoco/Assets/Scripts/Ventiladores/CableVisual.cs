using UnityEngine;

public class CableVisual : MonoBehaviour
{
    [Header("Puntos del cable")]
    public Transform puntoInicio;
    public Transform puntoFinal;

    [Header("Curvatura dinámica")]
    public float intensidadCurva = 1f; // Escala de la curvatura
    public int segmentos = 20;

    [Header("Offset manual del punto final")]
    public Vector2 offsetFinal = Vector2.zero; // ← NUEVO

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = segmentos + 1;
        lr.useWorldSpace = true;
    }

    void Update()
    {
        if (puntoInicio == null || puntoFinal == null) return;

        Vector3 p0 = puntoInicio.position;

        // Aplicamos el offset al punto final
        Vector3 p2 = puntoFinal.position + new Vector3(offsetFinal.x, offsetFinal.y, 0f);

        // Dirección entre los puntos
        Vector3 dir = (p2 - p0);
        float distancia = dir.magnitude;

        // Normal perpendicular 2D
        Vector3 normal = Vector3.Cross(dir.normalized, Vector3.forward);

        // Punto de control con curvatura dinámica
        float curvaturaDinamica = distancia * intensidadCurva * 0.5f;
        Vector3 p1 = (p0 + p2) / 2 + normal * curvaturaDinamica;

        for (int i = 0; i <= segmentos; i++)
        {
            float t = i / (float)segmentos;
            Vector3 puntoCurvo = Mathf.Pow(1 - t, 2) * p0 +
                                 2 * (1 - t) * t * p1 +
                                 Mathf.Pow(t, 2) * p2;
            lr.SetPosition(i, puntoCurvo);
        }
    }
}