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

    [Header("Indicadores individuales")]
    public GameObject indicadorRojo;
    public GameObject indicadorAzul;
    public GameObject indicadorRosa;

    [Header("Sprites de animación indicadores (2 sprites por indicador)")]
    public Sprite[] animRojo; // 2 sprites
    public Sprite[] animAzul; // 2 sprites
    public Sprite[] animRosa; // 2 sprites

    [Header("Indicador de error")]
    public GameObject errorSprite;

    [Header("Configuración")]
    public float threshold = 0.5f;

    [Header("Offsets de conexión")]
    public Vector3 offsetRojo;
    public Vector3 offsetAzul;
    public Vector3 offsetRosa;

    public bool escenaCompletada = false;

    private bool[] cableTocado = new bool[3];
    private float[] tiempoUltimoToque = new float[3];
    private float tiempoEspera = 10f;

    // Variables para animación indicadores
    private float animTimer = 0f;
    private int animFrame = 0;
    private float animInterval = 0.5f; // Cambio de sprite cada 0.5 segundos

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

        // Cambiar sprites si están cerca de CUALQUIER target
        CambiarSprite(cableRojo, offsetRojo, conectadoRojo, sueltoRojo);
        CambiarSprite(cableAzul, offsetAzul, conectadoAzul, sueltoAzul);
        CambiarSprite(cableRosa, offsetRosa, conectadoRosa, sueltoRosa);

        // Completar escena si todo está ok
        if (rojoOk && azulOk && rosaOk && !escenaCompletada && gameManager.instance != null)
        {
            gameManager.instance.CompletarEscena("OutInPut", true);
            escenaCompletada = true;
        }

        // Actualizar indicadores individuales con animación
        ActualizarIndicadores();
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

    void ActualizarIndicadores()
    {
        bool[] conectados = new bool[3];
        conectados[0] = Vector2.Distance(cableRojo.transform.position - offsetRojo, targetRojo.position) < threshold;
        conectados[1] = Vector2.Distance(cableAzul.transform.position - offsetAzul, targetAzul.position) < threshold;
        conectados[2] = Vector2.Distance(cableRosa.transform.position - offsetRosa, targetRosa.position) < threshold;

        GameObject[] indicadores = { indicadorRojo, indicadorAzul, indicadorRosa };
        Sprite[][] animSprites = { animRojo, animAzul, animRosa };

        // Actualizar timer para animación
        animTimer += Time.deltaTime;
        if (animTimer >= animInterval)
        {
            animTimer = 0f;
            animFrame = (animFrame + 1) % 2; // alterna entre 0 y 1
        }

        for (int i = 0; i < 3; i++)
        {
            if (escenaCompletada || conectados[i])
            {
                indicadores[i].SetActive(false);
                continue;
            }

            if (!cableTocado[i] || (Time.time - tiempoUltimoToque[i] > tiempoEspera))
            {
                cableTocado[i] = false;
                indicadores[i].SetActive(true);

                // Animar sprite y opacidad
                SpriteRenderer sr = indicadores[i].GetComponent<SpriteRenderer>();
                if (animSprites[i] != null && animSprites[i].Length == 2)
                {
                    sr.sprite = animSprites[i][animFrame];

                    // Opacidad que va y viene (fade in/out)
                    float alpha = Mathf.PingPong(Time.time * 2f, 1f);
                    Color c = sr.color;
                    c.a = alpha;
                    sr.color = c;
                }
            }
            else
            {
                indicadores[i].SetActive(false);
            }
        }
    }

    public void NotificarToque(GameObject cable)
    {
        int index = GetCableIndex(cable);
        if (index != -1)
        {
            cableTocado[index] = true;
            tiempoUltimoToque[index] = Time.time;

            GameObject[] indicadores = { indicadorRojo, indicadorAzul, indicadorRosa };
            indicadores[index].SetActive(false);
        }
    }

    int GetCableIndex(GameObject cable)
    {
        if (cable == cableRojo) return 0;
        if (cable == cableAzul) return 1;
        if (cable == cableRosa) return 2;
        return -1;
    }

    public bool GetEscenaCompletada()
    {
        return escenaCompletada;
    }
}