using UnityEngine;

public class BalanzaManager : MonoBehaviour
{
    [Range(0, 100)]
    public int temperatura = 50;

    public enum EstadoTermico { Frio, Caliente, Neutral }

    public bool esNeutral = false;
    private bool escenaCompletada = false;

    [Header("Sonidos OneShot")]
    public AudioClip sonidoFrio;
    public AudioClip sonidoCalor;

    [Header("Sonidos Loop")]
    public AudioClip sonidoFrioExtra;
    public AudioClip sonidoCalorExtra;

    [Header("Sonido gradual")]
    public AudioClip sonidoIntensidad;
    public float volumenMaximo = 1f;

    private AudioSource audioOneShot;
    private AudioSource audioLoop;
    private AudioSource audioIntensidad;

    private EstadoTermico ultimoEstado;

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length < 3)
        {
            Debug.LogError("Se necesitan 3 AudioSource en el GameObject");
            return;
        }

        audioOneShot = sources[0];
        audioLoop = sources[1];
        audioIntensidad = sources[2];

        audioLoop.loop = true;

        // Configuración del sonido gradual
        if (sonidoIntensidad != null)
        {
            audioIntensidad.clip = sonidoIntensidad;
            audioIntensidad.loop = true;
            audioIntensidad.Play();
        }

        ultimoEstado = ObtenerEstado();
        ActualizarSonidoLoop(ultimoEstado);
    }

    public EstadoTermico ObtenerEstado()
    {
        if (temperatura > 55)
            return EstadoTermico.Caliente;
        else if (temperatura < 50)
            return EstadoTermico.Frio;
        else
            return EstadoTermico.Neutral;
    }

    public void AumentarTemperatura()
    {
        temperatura = Mathf.Min(temperatura + 1, 100);
    }

    public void ReducirTemperatura()
    {
        temperatura = Mathf.Max(temperatura - 1, 0);
    }

    void Update()
    {
        EstadoTermico estadoActual = ObtenerEstado();

        if (estadoActual != ultimoEstado)
        {
            switch (estadoActual)
            {
                case EstadoTermico.Frio:
                    if (sonidoFrio != null) audioOneShot.PlayOneShot(sonidoFrio);
                    break;
                case EstadoTermico.Caliente:
                    if (sonidoCalor != null) audioOneShot.PlayOneShot(sonidoCalor);
                    break;
            }

            ActualizarSonidoLoop(estadoActual);
            ultimoEstado = estadoActual;
        }

        // Actualizar volumen gradual
        ActualizarVolumenGradual();

        esNeutral = estadoActual == EstadoTermico.Neutral;

        if (esNeutral && !escenaCompletada && gameManager.instance != null)
        {
            gameManager.instance.CompletarEscena("Ventiladores", true);
            escenaCompletada = true;
        }
    }

    void ActualizarSonidoLoop(EstadoTermico estado)
    {
        switch (estado)
        {
            case EstadoTermico.Frio:
                if (sonidoFrioExtra != null && audioLoop.clip != sonidoFrioExtra)
                {
                    audioLoop.clip = sonidoFrioExtra;
                    audioLoop.Play();
                }
                break;

            case EstadoTermico.Caliente:
                if (sonidoCalorExtra != null && audioLoop.clip != sonidoCalorExtra)
                {
                    audioLoop.clip = sonidoCalorExtra;
                    audioLoop.Play();
                }
                break;

            case EstadoTermico.Neutral:
                audioLoop.Stop();
                break;
        }
    }

    void ActualizarVolumenGradual()
    {
        if (audioIntensidad == null || sonidoIntensidad == null) return;

        // Distancia desde el centro (50)
        float distancia = Mathf.Abs(temperatura - 50) / 50f; // Va de 0 a 1
        float volumen = Mathf.Lerp(0f, volumenMaximo, distancia);
        audioIntensidad.volume = volumen;
    }
}