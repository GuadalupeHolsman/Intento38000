using UnityEngine;

public class CableManager : MonoBehaviour
{
    [Header("Cables")]
    public GameObject cableRojo;
    public GameObject cableAzul;
    public GameObject cableRosa;

    [Header("Targets")]
    public Transform targetRojo;
    public Transform targetAzul;
    public Transform targetRosa;

    [Header("Sprites de conexión")]
    public Sprite conectadoRojo;
    public Sprite conectadoAzul;
    public Sprite conectadoRosa;

    public Sprite sueltoRojo;
    public Sprite sueltoAzul;
    public Sprite sueltoRosa;

    [Header("Indicador de error")]
    public GameObject errorSprite;

    [Header("Configuración")]
    public float threshold = 0.5f;

    [Header("Offsets de conexión")]
    public Vector3 offsetRojo;
    public Vector3 offsetAzul;
    public Vector3 offsetRosa;

    private bool escenaCompletada = false;

    void Update()
    {
        // Verificar conexiones correctas considerando offset
        bool rojoOk = Vector2.Distance(cableRojo.transform.position - offsetRojo, targetRojo.position) < threshold;
        bool azulOk = Vector2.Distance(cableAzul.transform.position - offsetAzul, targetAzul.position) < threshold;
        bool rosaOk = Vector2.Distance(cableRosa.transform.position - offsetRosa, targetRosa.position) < threshold;

        // Snap a target correcto con offset
        if (rojoOk)
            cableRojo.transform.position = targetRojo.position + offsetRojo;

        if (azulOk)
            cableAzul.transform.position = targetAzul.position + offsetAzul;

        if (rosaOk)
            cableRosa.transform.position = targetRosa.position + offsetRosa;

        // Mostrar u ocultar sprite de error
        //errorSprite.SetActive(!(rojoOk && azulOk && rosaOk));

        //Marcar como completa la escena
        if (rojoOk && azulOk && rosaOk && !escenaCompletada && gameManager.instance != null)
        {
            gameManager.instance.CompletarEscena("OutInPut", true);
            escenaCompletada = true;
        }

        // Cambiar sprites si están cerca de CUALQUIER target
        CambiarSprite(cableRojo, offsetRojo, conectadoRojo, sueltoRojo);
        CambiarSprite(cableAzul, offsetAzul, conectadoAzul, sueltoAzul);
        CambiarSprite(cableRosa, offsetRosa, conectadoRosa, sueltoRosa);
    }

    void CambiarSprite(GameObject cable, Vector3 offset, Sprite spriteConectado, Sprite spriteSuelto)
    {
        Vector2 cablePos = cable.transform.position - offset;

        float d1 = Vector2.Distance(cablePos, targetRojo.position);
        float d2 = Vector2.Distance(cablePos, targetAzul.position);
        float d3 = Vector2.Distance(cablePos, targetRosa.position);

        SpriteRenderer sr = cable.GetComponent<SpriteRenderer>();

        if (d1 < threshold || d2 < threshold || d3 < threshold)
        {
            sr.sprite = spriteConectado;
        }
        else
        {
            sr.sprite = spriteSuelto;
        }
    }
}