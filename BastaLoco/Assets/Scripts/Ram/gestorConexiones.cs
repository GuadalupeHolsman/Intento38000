using UnityEngine;
using System.Collections.Generic;

public class gestorConexiones : MonoBehaviour
{
    [System.Serializable]
    public class PasoDeConexion
    {
        public ArrastrarCable2D cable;
        public List<Animator> bateriasCambiar;
        public List<int> nuevosEstadosBaterias;

        public List<Animator> extremosCambiar;
        public List<int> nuevosEstadosExtremos;

        public List<DibujarLinea> cablesCambiar;
        public List<Color> nuevosColoresCables;

    }

    public List<PasoDeConexion> pasos;
    private int pasoActual = 0;
    public bool conexionesCompletas = false;

    void Update()
    {
        if (pasoActual >= pasos.Count) return;

        var paso = pasos[pasoActual];

        if (paso.cable.conectado)
        {
            // Cambiar estados de baterías
            for (int i = 0; i < paso.bateriasCambiar.Count; i++)
            {
                paso.bateriasCambiar[i].SetInteger("estado", paso.nuevosEstadosBaterias[i]);
            }

            // Cambiar estados de extremos
            for (int i = 0; i < paso.extremosCambiar.Count; i++)
            {
                paso.extremosCambiar[i].SetInteger("estado", paso.nuevosEstadosExtremos[i]);
            }

            // Cambiar colores de cables
            for (int i = 0; i < paso.cablesCambiar.Count; i++)
            {
                paso.cablesCambiar[i].SetColor(paso.nuevosColoresCables[i]);
            }

            pasoActual++;

            if (pasoActual >= pasos.Count)
                {
                    conexionesCompletas = true;
                }
        }
    }
}
