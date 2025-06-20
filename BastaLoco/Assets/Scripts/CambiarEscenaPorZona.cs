using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenaPorZona : MonoBehaviour
{
    public string nombreEscena;

    void OnMouseDown()  // Para clic (funciona en PC)
    {
        CambiarEscena();
    }

    void OnMouseUpAsButton()  // Alternativa para tacto
    {
        CambiarEscena();
    }

    void CambiarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}
