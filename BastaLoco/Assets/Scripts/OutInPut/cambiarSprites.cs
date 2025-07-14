using UnityEngine;

public class cambiarSprites : MonoBehaviour
{
    public CableManager cableManager; // Asigná el script CableManager
    public GameObject spriteParaOcultar; // Sprite que se oculta
    public SpriteRenderer spriteParaMostrar; // Sprite que se activa

    private bool cambioRealizado = false;

    void Update()
    {
        if (cableManager != null && cableManager.escenaCompletada && !cambioRealizado)
        {
            if (spriteParaOcultar != null)
                spriteParaOcultar.SetActive(false);

            if (spriteParaMostrar != null)
                spriteParaMostrar.enabled = true;

            cambioRealizado = true;
        }
    }
}
