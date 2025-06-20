using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenaAlTocar : MonoBehaviour
{
    public DragTapa dragTapa; // Referencia al script de la tapa
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (dragTapa == null || !dragTapa.YaCayo())
            return; // 👉 si no cayó la tapa, no hace nada

        // TOQUE táctil
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPos = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
            touchPos.z = 0;

            Collider2D hit = Physics2D.OverlapPoint(touchPos);
            if (hit != null && hit.gameObject == gameObject)
            {
                CambiarEscena();
            }
        }

        // CLIC de mouse
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null && hit.gameObject == gameObject)
            {
                CambiarEscena();
            }
        }
    }

   void CambiarEscena()
{
    Debug.Log("Cambiando a escena CloseUpPc");
    SceneManager.LoadScene("CloseUpPc");
}
}
