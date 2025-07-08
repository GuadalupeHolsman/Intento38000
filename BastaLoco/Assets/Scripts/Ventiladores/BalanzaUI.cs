using UnityEngine;
using UnityEngine.UI;

public class BalanzaUI : MonoBehaviour
{
    public BalanzaManager balanza; // Arrastrás aquí el GameObject con el script BalanzaManager

    [Header("Referencias UI")]
    public Image fondo;             // Arrastrás el objeto "Fondo" (con componente Image)
    public Sprite spriteFrio;      // Sprite de la balanza congelada
    public Sprite spriteCaliente;  // Sprite de la balanza caliente
    public Sprite spriteNeutral;   // Sprite de la balanza equilibrada

    public RectTransform indicador;     // Arrastrás el objeto "Indicador" (la flecha)

    [Header("Ajustes de movimiento")]
    public float anguloMaximo = 30f;     // Hasta qué ángulo gira la flecha (de -30 a 30)
    public float suavizado = 5f;         // Velocidad con la que la flecha se suaviza

    private float anguloActual = 0f;

    void Update()
    {
        if (balanza == null) return;

        // 1. Cambiar el sprite del fondo según el estado
        switch (balanza.ObtenerEstado())
        {
            case BalanzaManager.EstadoTermico.Frio:
                fondo.sprite = spriteFrio;
                break;
            case BalanzaManager.EstadoTermico.Caliente:
                fondo.sprite = spriteCaliente;
                break;
            case BalanzaManager.EstadoTermico.Neutral:
                fondo.sprite = spriteNeutral;
                break;
        }

        // 2. Calcular el ángulo objetivo según temperatura
        float t = Mathf.InverseLerp(0, 100, balanza.temperatura); // 0 a 1
        float anguloObjetivo = Mathf.Lerp(-anguloMaximo, anguloMaximo, t); // -30 a 30

        // 3. Suavizar la rotación de la flecha
        anguloActual = Mathf.Lerp(anguloActual, anguloObjetivo, Time.deltaTime * suavizado);
        indicador.localRotation = Quaternion.Euler(0, 0, -anguloActual); // girar en eje Z (negativo para que 0 esté en el centro)
    }
}