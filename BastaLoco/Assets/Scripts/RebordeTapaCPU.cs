using UnityEngine;

public class ActivarReborde : MonoBehaviour
{
    public GameObject rebordeTapa;

    void Start()
    {
        rebordeTapa.SetActive(false);
        Invoke("MostrarReborde", 10f);
    }

    void MostrarReborde()
    {
        rebordeTapa.SetActive(true);
    }
}