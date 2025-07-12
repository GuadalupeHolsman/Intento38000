using UnityEngine;

public class CableGenerator2D : MonoBehaviour
{
    public Transform puntoA; // Se mueve
    public Transform puntoB; // Fijo
    public GameObject prefabEslabon;
    public int cantidadEslabones = 15;
    public float largoTotal = 5f;

    void Start()
    {
        GenerarCuerda();
    }

    void GenerarCuerda()
    {
        Vector2 direccion = (puntoA.position - puntoB.position).normalized;

        // Menor distancia para superponer un poco los eslabones
        float distancia = (largoTotal / cantidadEslabones) * 0.8f;

        Rigidbody2D cuerpoAnterior = puntoB.GetComponent<Rigidbody2D>();
        GameObject ultimoEslabon = null;

        for (int i = 0; i < cantidadEslabones; i++)
        {
            Vector2 posicion = (Vector2)puntoB.position + direccion * distancia * (i + 1);
            GameObject eslabon = Instantiate(prefabEslabon, posicion, Quaternion.identity);

            Rigidbody2D rb = eslabon.GetComponent<Rigidbody2D>();

            // Hinge Joint para rotación libre
            HingeJoint2D hinge = eslabon.GetComponent<HingeJoint2D>();
            hinge.connectedBody = cuerpoAnterior;

            // Distance Joint para limitar estiramiento
            DistanceJoint2D distanciaJoint = eslabon.AddComponent<DistanceJoint2D>();
            distanciaJoint.connectedBody = cuerpoAnterior;
            distanciaJoint.autoConfigureDistance = false;
            distanciaJoint.distance = distancia;
            distanciaJoint.enableCollision = false; // evita fuerzas extra

            cuerpoAnterior = rb;
            ultimoEslabon = eslabon;
        }

        // Conectar el último eslabón al puntoA
        if (ultimoEslabon != null)
        {
            Rigidbody2D rbUltimo = ultimoEslabon.GetComponent<Rigidbody2D>();

            HingeJoint2D jointFinal = puntoA.GetComponent<HingeJoint2D>();
            jointFinal.connectedBody = rbUltimo;

            DistanceJoint2D distanciaFinal = puntoA.gameObject.AddComponent<DistanceJoint2D>();
            distanciaFinal.connectedBody = rbUltimo;
            distanciaFinal.autoConfigureDistance = false;
            distanciaFinal.distance = distancia;
            distanciaFinal.enableCollision = false;
        }
    }
}