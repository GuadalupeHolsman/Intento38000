using UnityEngine;
using System.Collections;

public class movimientoBolita : MonoBehaviour
{
    public Transform centro;
    public Transform posicionInicio;
    public float velocidadMovimiento = 3f;
    public float radioX = 2f;
    public float radioY = 1f;
    public float velocidadGiro = 2f;
    public float duracion = 5f;
    public float profundidadZ = 0.3f;
    public float tiempoEspera = 1f;

    private float angulo = 0f;
    private float tiempoGirando = 0f;
    private SpriteRenderer sr;
    private Vector3 posicionOriginal;
    private SecuenciaDejaBolita controladorEstado;

    private enum Estado { Esperando, MoviendoAHInicio, Girando, Reiniciando }
    private Estado estado = Estado.Esperando;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        posicionOriginal = transform.position;
        sr.enabled = false;

        StartCoroutine(MonitorearEstados());
    }
    IEnumerator MonitorearEstados()
    {
        while (true)
        {
            SecuenciaDejaBolita[] personajes = FindObjectsByType<SecuenciaDejaBolita>(FindObjectsSortMode.None);
            bool algunoEn3 = false;

            foreach (var personaje in personajes)
            {
                if (personaje.EstadoActual == 3)
                {
                    algunoEn3 = true;
                    break;
                }
            }

            if (algunoEn3 && estado == Estado.Esperando)
            {
                sr.enabled = true;
                estado = Estado.MoviendoAHInicio;
            }

            yield return new WaitForSeconds(0.2f); // revisar 5 veces por segundo
        }
    }

    IEnumerator EsperarEstadoInicial()
    {
        // Esperamos a que controladorEstado esté en estado 3
        while (controladorEstado == null || controladorEstado.EstadoActual != 3)
        {
            yield return null;
        }

        // Una vez que llega a estado 3, activamos la bolita
        sr.enabled = true;
        enabled = true;
        estado = Estado.MoviendoAHInicio;
    }

    void Update()
    {
        switch (estado)
        {
            case Estado.MoviendoAHInicio:
                MoverAHInicio();
                break;

            case Estado.Girando:
                Girar();
                break;
        }
    }

    void MoverAHInicio()
    {
        transform.position = Vector3.MoveTowards(transform.position, posicionInicio.position, velocidadMovimiento * Time.deltaTime);

        if (Vector3.Distance(transform.position, posicionInicio.position) < 0.01f)
        {
            Vector3 dir = (transform.position - centro.position).normalized;
            angulo = Mathf.Atan2(dir.y, dir.x);
            tiempoGirando = 0f;
            estado = Estado.Girando;
        }
    }

    void Girar()
    {
        tiempoGirando += Time.deltaTime;

        if (tiempoGirando > duracion)
        {
            StartCoroutine(DesaparecerYReiniciar());
            estado = Estado.Reiniciando;
            return;
        }

        angulo -= velocidadGiro * Time.deltaTime;

        float x = centro.position.x + Mathf.Cos(angulo) * radioX;
        float y = centro.position.y + Mathf.Sin(angulo) * radioY;
        float z = Mathf.Sin(angulo) * profundidadZ;

        transform.position = new Vector3(x, y, z);
    }

    IEnumerator DesaparecerYReiniciar()
    {
        sr.enabled = false;
        yield return new WaitForSeconds(tiempoEspera);
        transform.position = posicionOriginal;
        estado = Estado.Esperando; // Vuelve a esperar el estado 3
    }

    public void IniciarDesdeGiro()
    {
        Vector3 dir = (transform.position - centro.position).normalized;
        angulo = Mathf.Atan2(dir.y, dir.x);
        tiempoGirando = 0f;
        estado = Estado.Girando;
    }

    IEnumerator RevisarEstado()
    {
        while (controladorEstado.EstadoActual != 3)
        {
            yield return null;
        }

        Debug.Log("Estado llegó a 3, activando bolita.");

        sr.enabled = true;
        estado = Estado.MoviendoAHInicio;
    }

}
