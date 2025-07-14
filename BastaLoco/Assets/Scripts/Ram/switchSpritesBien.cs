using UnityEngine;

public class switchSpritesBien : MonoBehaviour
{
    public gestorConexiones gestor;
    public SpriteRenderer[] spritesAOcultar;
    public SpriteRenderer[] spritesAMostrar;

    private bool cambioHecho = false;

    void Update()
    {
        if (gestor == null) return;

        if (gestor.conexionesCompletas && !cambioHecho)
        {
            // Ocultar
            foreach (var sr in spritesAOcultar)
            {
                if (sr != null) sr.enabled = false;
            }

            // Mostrar
            foreach (var sr in spritesAMostrar)
            {
                if (sr != null) sr.enabled = true;
            }

            cambioHecho = true; // para que no lo haga cada frame
        }
    }
}
