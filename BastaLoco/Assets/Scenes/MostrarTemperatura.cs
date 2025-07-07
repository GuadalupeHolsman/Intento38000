using UnityEngine;
using TMPro;

public class MostrarTemperatura : MonoBehaviour
{
    public BalanzaManager balanza;
    private TextMeshProUGUI textoTMP;

    void Start()
    {
        textoTMP = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (balanza != null)
        {
            int temp = balanza.temperatura;
            string estado = balanza.ObtenerEstado().ToString();

            // Actualizar el texto con la temperatura y el estado
            textoTMP.text = "Temperatura: " + temp + "° (" + estado + ")";
        }
    }
}