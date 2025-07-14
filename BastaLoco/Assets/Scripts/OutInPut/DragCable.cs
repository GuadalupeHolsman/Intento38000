using UnityEngine;

public class DragCable : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;

    private CableManager cableManager;

    void Start()
    {
        cableManager = FindObjectOfType<CableManager>();
    }

    void Update()
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

                if (cableManager != null)
                {
                    cableManager.NotificarToque(gameObject);
                }
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

    private LayerMask GetCableLayerMask()
    {
        int ignoreLayer = LayerMask.NameToLayer("NoRaycast");
        return ~(1 << ignoreLayer); // invierte la máscara para ignorar esa capa
    }
}
