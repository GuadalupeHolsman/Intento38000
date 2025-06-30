using UnityEngine;

public class EstadosBateria : MonoBehaviour
{
    public int estadoInicial = 0;

    void Start()
    {
        GetComponent<Animator>().SetInteger("estado", estadoInicial);
    }
}
