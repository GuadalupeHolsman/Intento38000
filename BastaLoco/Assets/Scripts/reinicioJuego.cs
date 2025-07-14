using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class reinicioJuego : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public SpriteRenderer fadeSpriteRenderer;
    public float fadeDuration = 1.5f;

    private void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        // Asegurar que empiece transparente
        if (fadeSpriteRenderer != null)
        {
            Color c = fadeSpriteRenderer.color;
            c.a = 0f;
            fadeSpriteRenderer.color = c;
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(FadeAndLoadScene());
    }

    IEnumerator FadeAndLoadScene()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            if (fadeSpriteRenderer != null)
            {
                Color c = fadeSpriteRenderer.color;
                c.a = alpha;
                fadeSpriteRenderer.color = c;
            }

            yield return null;
        }

        // Reiniciar gameManager
        if (gameManager.instance != null)
        {
            gameManager.instance.ReiniciarProgreso();
            gameManager.instance.personajeYaAparecio = false;
        }

        SceneManager.LoadScene("Inicio");
    }
}
