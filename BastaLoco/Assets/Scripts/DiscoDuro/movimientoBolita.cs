using UnityEngine;
using System.Collections;

public class movimientoBolita : MonoBehaviour
{
    public Transform centro;
    public Transform posicionInicio;     // Donde empieza el giro
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

    public bool llegoAPosicionInicio = false;
    private enum Estado { MoviendoAHInicio, Girando, Reiniciando }
    private Estado estado = Estado.MoviendoAHInicio;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        posicionOriginal = transform.position; // Guardamos la posición inicial
        sr.enabled = true;
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
        transform.position = posicionOriginal; // volver a donde estaba al inicio
        sr.enabled = true;
        estado = Estado.MoviendoAHInicio;
    }
    public void IniciarDesdeGiro()
{
    llegoAPosicionInicio = true;
    Vector3 dir = (transform.position - centro.position).normalized;
    angulo = Mathf.Atan2(dir.y, dir.x);
    tiempoGirando = 0f;
}
    
}
