using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    // Mapa de progreso: clave = nombre de la escena, valor = true si fue completada bien
    public Dictionary<string, bool> progreso = new Dictionary<string, bool>();

    // Escenas a controlar
    public List<string> escenasAControlar;

    // Nombre de las escenas finales
    public string escenaFinalLogrado = "FinalLogrado";
    public string escenaFinalError = "FinalError";
    public bool personajeYaAparecio = false;

    void Awake()
    {
        // Singleton: aseguramos que solo exista una instancia
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método que se llama cuando una escena se completa
    public void CompletarEscena(string nombreEscena, bool correcto)
    {
        progreso[nombreEscena] = correcto;

        // Verificamos si ya se completaron todas
        if (SeCompletaronTodas())
        {
            if (TodasCorrectas())
                SceneManager.LoadScene(escenaFinalLogrado);
            else
                SceneManager.LoadScene(escenaFinalError);
        }
    }

    private bool SeCompletaronTodas()
    {
        foreach (string escena in escenasAControlar)
        {
            if (!progreso.ContainsKey(escena))
                return false;
        }
        return true;
    }

    private bool TodasCorrectas()
    {
        foreach (bool valor in progreso.Values)
        {
            if (!valor) return false;
        }
        return true;
    }

    public void ReiniciarProgreso()
    {
        progreso.Clear();
    }
}
