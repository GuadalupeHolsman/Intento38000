using UnityEngine;

public class DibujarLinea : MonoBehaviour
{
    //public Transform extremoFijo;
    //public Transform extremoMovil;
    public Transform[] puntosLinea; // Ahora soporta varios puntos (mínimo 2)
    private LineRenderer linea;
    private Color colorLinea;

    void Start()
    {
        ColorUtility.TryParseHtmlString("#060428", out colorLinea);

        linea = GetComponent<LineRenderer>();
        //linea.positionCount = 2;
        linea.positionCount = puntosLinea.Length;
        linea.material = new Material(Shader.Find("Sprites/Default"));
        linea.startColor = colorLinea;
        linea.endColor = colorLinea;
        linea.startWidth = 0.1f;
        linea.endWidth = 0.1f;
    
    }

    void Update()
    {
        /* if (linea != null && extremoFijo != null && extremoMovil != null)
        {
            linea.SetPosition(0, extremoFijo.position);
            linea.SetPosition(1, extremoMovil.position);
        } */
        for (int i = 0; i < puntosLinea.Length; i++)
        {
            linea.SetPosition(i, puntosLinea[i].position);
        }
    }
}
