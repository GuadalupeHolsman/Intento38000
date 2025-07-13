using UnityEngine;
using System.Collections.Generic;

public class CablePadre : MonoBehaviour
{
    public Transform[] puntosCable;
    private LineRenderer linea;
    private Color colorLinea;
    public PolygonCollider2D zonaValida;

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

    public void EliminarPunto(Transform punto)
    {
        List<Transform> nuevaLista = new List<Transform>(puntosCable);
        nuevaLista.Remove(punto);
        puntosCable = nuevaLista.ToArray();
        linea.positionCount = puntosCable.Length;
    }

    void OnDrawGizmosSelected()
    {
        if (zonaValida != null)
        {
            Gizmos.color = Color.cyan;  // Elegí el color que prefieras
            Vector3 pos = zonaValida.bounds.center;
            Vector3 size = zonaValida.bounds.size;
            Gizmos.DrawWireCube(pos, size);
        }
    }
}
