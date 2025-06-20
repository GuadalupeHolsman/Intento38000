using UnityEngine;

public class ActivarReborde : MonoBehaviour
{
    public GameObject rebordeTapa;
    public DragTapa dragTapa; // Referencia al script DragTapa

    void Start()
    {
        rebordeTapa.SetActive(false);
        Invoke("MostrarReborde", 10f);
    }

    void MostrarReborde()
    {
        // Solo mostrar el reborde si todavía no cayó
        if (dragTapa != null && !dragTapa.YaCayo())
        {
            rebordeTapa.SetActive(true);
        }
    }
}