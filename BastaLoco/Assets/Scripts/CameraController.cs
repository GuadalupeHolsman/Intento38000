using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float minZoom = 3f;
    public float maxZoom = 10f;
    public Transform background;

    private Camera cam;
    private Vector3 originalBackgroundScale;
    private float initialOrthoSize;

    void Start()
    {
        cam = GetComponent<Camera>();
        initialOrthoSize = cam.orthographicSize;

        if (background != null)
        {
            originalBackgroundScale = background.localScale;
        }
    }

    void Update()
    {
        HandleMouseZoom();
        HandleTouchZoom();
        AdjustBackgroundScale();
    }

    void HandleMouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    void HandleTouchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

            float prevMag = (touch0Prev - touch1Prev).magnitude;
            float currMag = (touch0.position - touch1.position).magnitude;

            float difference = prevMag - currMag;

            cam.orthographicSize += difference * zoomSpeed * 0.01f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    void AdjustBackgroundScale()
    {
        if (background != null)
        {
            float factor = cam.orthographicSize / initialOrthoSize;
            background.localScale = originalBackgroundScale / factor;
        }
    }
}