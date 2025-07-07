using UnityEngine;

public class DragCableVentiladores : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;

    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    void OnMouseDrag()
    {
        if (dragging)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            pos.z = 0;
            transform.position = pos;
        }
    }

    void OnMouseUp()
    {
        dragging = false;
    }
}