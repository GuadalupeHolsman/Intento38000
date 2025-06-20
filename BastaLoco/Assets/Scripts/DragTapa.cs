using System.Collections;
using UnityEngine;

public class DragTapa : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;
    private Rigidbody2D rb;
    private Camera cam;

    public GameObject rebordeTapaCPU;
    public GameObject point;

    private bool yaCayo = false;
    private Vector3 pointOriginalPosition;
    private Vector3 pointOriginalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        cam = Camera.main;

        if (point != null)
        {
            pointOriginalPosition = point.transform.position;
            pointOriginalScale = point.transform.localScale;
            point.SetActive(false);
        }

        if (rebordeTapaCPU != null)
            rebordeTapaCPU.SetActive(false);
    }

    void Update()
    {
        // MOUSE
        if (Input.GetMouseButtonDown(0) && IsPointerOverMe(Input.mousePosition))
            StartDragging(Input.mousePosition);

        if (dragging)
        {
            if (Input.GetMouseButton(0))
                Drag(Input.mousePosition);
            else if (Input.GetMouseButtonUp(0))
                StopDragging();
        }

        // TOUCH
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && IsPointerOverMe(touch.position))
                StartDragging(touch.position);

            if (dragging && touch.phase == TouchPhase.Moved)
                Drag(touch.position);

            if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                StopDragging();
        }
    }

    bool IsPointerOverMe(Vector3 pointerPosition)
    {
        Vector3 worldPoint = cam.ScreenToWorldPoint(pointerPosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPoint);
        return hit != null && hit.gameObject == gameObject;
    }

    void StartDragging(Vector3 pointerPosition)
    {
        rb.gravityScale = 0;
        dragging = true;
        offset = transform.position - cam.ScreenToWorldPoint(pointerPosition);

        if (rebordeTapaCPU != null && !yaCayo)
            rebordeTapaCPU.SetActive(false);
    }

    void Drag(Vector3 pointerPosition)
    {
        Vector3 newPos = cam.ScreenToWorldPoint(pointerPosition) + offset;
        newPos.z = 0;
        transform.position = newPos;
    }

    void StopDragging()
    {
        dragging = false;
        rb.gravityScale = 1;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);

        if (collision.gameObject.name == "SueloDetector" && !yaCayo)
        {
            yaCayo = true;

            if (point != null)
            {
                StartCoroutine(ActivarPointConDelay());
            }
            else
            {
                Debug.Log("El objeto POINT no está asignado");
            }
        }
    }

    IEnumerator ActivarPointConDelay()
    {
        yield return new WaitForSeconds(2f);

        point.SetActive(true);
        point.transform.position = pointOriginalPosition;
        point.transform.localScale = pointOriginalScale;
        point.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // 👉 Método público para consultar si ya cayó
    public bool YaCayo()
    {
        return yaCayo;
    }
}