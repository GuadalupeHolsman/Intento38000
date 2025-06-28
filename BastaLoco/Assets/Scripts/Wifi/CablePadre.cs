using UnityEngine;

public class CablePadre : MonoBehaviour
{
    public Transform[] puntosCable; 
    private LineRenderer linea;
    private Color colorLinea;

    void Start()
    {
        ColorUtility.TryParseHtmlString("#53e56e", out colorLinea);

        linea = GetComponent<LineRenderer>();
        linea.positionCount = puntosCable.Length;
        linea.material = new Material(Shader.Find("Sprites/Default"));
        linea.startColor = colorLinea;
        linea.endColor = colorLinea;
        linea.startWidth = 0.03f;
        linea.endWidth = 0.03f;
    
    }

    void Update()
    {
        for (int i = 0; i < puntosCable.Length; i++)
        {
            linea.SetPosition(i, puntosCable[i].position);
        }
    }
}
