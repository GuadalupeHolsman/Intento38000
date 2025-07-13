using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DetectorConexion : MonoBehaviour
{
    public Transform posicionCorrecta;
    public GameObject bolitaPrefab;
    public GameObject rayaPrefab;
    public float tolerancia = 0.5f;

    public bool conectado = false;
    private Vector3 offset;
    private bool arrastrando = false;
    private Camera camara;
    private bool bienConectado = false;
    private bool malConectado = false;
    private bool escenaCompletada = false;
    private float esperaAntesError = 4f;

    void Start()
    {
        camara = Camera.main;
    }

    void OnMouseDown()
    {
        if (conectado) return;

        offset = transform.position - camara.ScreenToWorldPoint(Input.mousePosition);
        arrastrando = true;
    }

    void OnMouseDrag()
    {
        if (arrastrando && !conectado)
        {
            Vector3 mousePosition = camara.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x + offset.x, mousePosition.y + offset.y, 0f);
        }
    }

    void OnMouseUp()
    {
        if (conectado) return;

        arrastrando = false;

        float distancia = Vector2.Distance(transform.position, posicionCorrecta.position);

        if (distancia < 0.1f)
        {
            // Bien conectado
            Instantiate(bolitaPrefab, posicionCorrecta.position, Quaternion.identity);
            transform.position = posicionCorrecta.position; // imán
            conectado = true;
            bienConectado = true;
        }
        else if (distancia < tolerancia)
        {
            // Cerca pero mal
            Instantiate(rayaPrefab, transform.position, Quaternion.identity);
            malConectado = true;
            // Se queda en el lugar actual como si se pegara igual
            conectado = true;

        }

        if (bienConectado && !escenaCompletada && gameManager.instance != null)
        {
            gameManager.instance.CompletarEscena("Procesador", true);
            escenaCompletada = true;
        }

        if (malConectado)
        {
            StartCoroutine(EscenaError());
        }

    }

    IEnumerator EscenaError()
    {
        yield return new WaitForSeconds(esperaAntesError);
        SceneManager.LoadScene("Error");
    }
}