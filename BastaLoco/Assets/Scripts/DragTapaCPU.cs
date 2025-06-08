using UnityEngine;
using System.Collections;

public class ArrastrarTapa : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private bool arrastrando = false;

    public GameObject bordeTitilante; // Arrastrá acá el borde en el Inspector

    void Start()
    {
        cam = Camera.main;

        if (bordeTitilante != null)
            bordeTitilante.SetActive(false);
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // 👉 Mouse
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                offset = transform.position - mousePos;
                arrastrando = true;
            }
        }

        if (Input.GetMouseButton(0) && arrastrando)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos + offset;
        }

        if (Input.GetMouseButtonUp(0) && arrastrando)
        {
            arrastrando = false;
            StartCoroutine(EsperarYActivarBorde());
        }

#else
        // 👉 Touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = cam.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    RaycastHit2D hit = Physics2D.Raycast(touch.position, Vector2.zero);
                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        offset = transform.position - touchPos;
                        arrastrando = true;
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (arrastrando)
                        transform.position = touchPos + offset;
                    break;

                case TouchPhase.Ended:
                    if (arrastrando)
                    {
                        arrastrando = false;
                        StartCoroutine(EsperarYActivarBorde());
                    }
                    break;
            }
        }
#endif
    }

    IEnumerator EsperarYActivarBorde()
    {
        yield return new WaitForSeconds(5f);
        if (bordeTitilante != null)
        {
            bordeTitilante.SetActive(true);
        }
    }
}