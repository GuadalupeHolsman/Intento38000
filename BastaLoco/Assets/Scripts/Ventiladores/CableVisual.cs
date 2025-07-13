using UnityEngine;

public class CableVisual : MonoBehaviour
{
    [Header("Puntos del cable")]
    public Transform puntoInicio;
    public Transform puntoFinal;

    [Header("Curvatura dinámica")]
    public float intensidadCurva = 1f; // Escala de la curvatura
    public int segmentos = 20;

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
        Vector3 p2 = puntoFinal.position;

        // Dirección entre los puntos
        Vector3 dir = (p2 - p0);
        float distancia = dir.magnitude;

        // Calculamos una normal 2D (perpendicular)
        Vector3 normal = Vector3.Cross(dir.normalized, Vector3.forward);

        // Punto de control en el medio + curvatura que aumenta con la distancia
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