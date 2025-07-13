using UnityEngine;

public class BalanzaManager : MonoBehaviour
{
    [Range(0, 100)]
    public int temperatura = 50; // Comienza en un valor neutral (50)
    public float velocidadCambio = 1f; // Velocidad de cambio de temperatura

    public enum EstadoTermico { Frio, Caliente, Neutral }

    public bool esNeutral = false;
    private bool escenaCompletada = false;

    public EstadoTermico ObtenerEstado()
    {
        if (temperatura > 55)
            return EstadoTermico.Caliente;
        else if (temperatura < 45)
            return EstadoTermico.Frio;
        else
            return EstadoTermico.Neutral;
    }

    // Función para aumentar la temperatura
    public void AumentarTemperatura()
    {
        temperatura += 1;
        if (temperatura > 100) temperatura = 100;
    }

    public void ReducirTemperatura()
    {
        temperatura -= 1;
        if (temperatura < 0) temperatura = 0;
    }

    void Update()
    {
        esNeutral = ObtenerEstado() == EstadoTermico.Neutral;
        if (esNeutral && !escenaCompletada && gameManager.instance != null)
        {
            gameManager.instance.CompletarEscena("Ventiladores", true);
            escenaCompletada = true;
        }
    }
}