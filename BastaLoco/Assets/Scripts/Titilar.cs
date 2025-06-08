using UnityEngine;

public class Titilar : MonoBehaviour
{
    public float velocidad = 2f;
    private SpriteRenderer sr;

    void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy || sr == null) return;

        float alpha = Mathf.Lerp(0.3f, 1f, Mathf.PingPong(Time.time * velocidad, 1));
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}