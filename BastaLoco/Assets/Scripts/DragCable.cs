using UnityEngine;

public class DragCable : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;

    void Update()
    {
        // Táctil
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    RaycastHit2D hitTouch = Physics2D.Raycast(touchPos, Vector2.zero, Mathf.Infinity, GetCableLayerMask());
                    if (hitTouch.collider != null && hitTouch.collider.gameObject == gameObject)
                    {
                        offset = transform.position - touchPos;
                        isDragging = true;
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (isDragging)
                    {
                        transform.position = touchPos + offset;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }

        // Mouse (Editor)
        else if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Collider2D hit = Physics2D.OverlapPoint(mousePos, GetCableLayerMask());
                if (hit != null && hit.gameObject == gameObject)
                {
                    offset = transform.position - mousePos;
                    isDragging = true;
                }
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                transform.position = mousePos + offset;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }
    }

    private LayerMask GetCableLayerMask()
    {
        int ignoreLayer = LayerMask.NameToLayer("NoRaycast");
        return ~(1 << ignoreLayer); // invierte la máscara para ignorar esa capa
    }
}