using UnityEngine;
using System.Collections.Generic;

public class DibujarLinea : MonoBehaviour
{
    public Transform[] puntosLinea;

    [Header("Color del cable")]
    [SerializeField] private Color colorLinea = Color.white;

    private LineRenderer linea;

    void Start()
    {
        linea = GetComponent<LineRenderer>();
        linea.positionCount = puntosLinea.Length;

        // NO modificar el material en tiempo de ejecución si ya se asignó en el Inspector
        linea.startColor = colorLinea;
        linea.endColor = colorLinea;
        linea.startWidth = 0.1f;
        linea.endWidth = 0.1f;
    }

    void Update()
    {
        for (int i = 0; i < puntosLinea.Length; i++)
        {
            linea.SetPosition(i, puntosLinea[i].position);
        }
    }

    public void SetColor(Color nuevoColor)
    { 
        colorLinea = nuevoColor;
        if (linea != null)
        {
            linea.startColor = nuevoColor;
            linea.endColor = nuevoColor;
        }
    }
}
