using UnityEngine;
using System.Collections.Generic;

public class DibujarLinea : MonoBehaviour
{
    
    public Transform[] puntosLinea; // Ahora soporta varios puntos (minimo 2)
    private LineRenderer linea;
    private Color colorLinea;

    void Start()
    {
        ColorUtility.TryParseHtmlString("#060428", out colorLinea);

        linea = GetComponent<LineRenderer>();
        linea.positionCount = puntosLinea.Length;
        linea.material = new Material(Shader.Find("Sprites/Default"));
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
