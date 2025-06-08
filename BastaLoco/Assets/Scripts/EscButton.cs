using UnityEngine;
using UnityEngine.SceneManagement;

public class EscButton : MonoBehaviour
{
    public string nombreEscenaInicio = "Inicio";

    private void Update()
    {
        // Soporte para tecla Escape (opcional)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IrAEscenaInicio();
        }

        // Soporte táctil
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            DetectarToque(Input.GetTouch(0).position);
        }

        // Soporte mouse (click)
        if (Input.GetMouseButtonDown(0))
        {
            DetectarToque(Input.mousePosition);
        }
    }

    void DetectarToque(Vector2 posicionPantalla)
    {
        Vector2 posicionMundo = Camera.main.ScreenToWorldPoint(posicionPantalla);
        RaycastHit2D hit = Physics2D.Raycast(posicionMundo, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            IrAEscenaInicio();
        }
    }

    void IrAEscenaInicio()
    {
        SceneManager.LoadScene(nombreEscenaInicio);
    }
}