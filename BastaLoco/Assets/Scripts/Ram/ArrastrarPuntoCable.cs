using UnityEngine;

public class ArrastrarPuntoCable : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private bool arrastrando = false;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        arrastrando = true;
        offset = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        arrastrando = false;
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
